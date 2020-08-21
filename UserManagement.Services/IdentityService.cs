using UserManagement.Domain.Entities;
using UserManagement.Domain.RepositoryInterfaces;
using UserManagement.Services.Options;
using UserManagement.Services.Boundaries;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Generic;
using UserManagement.Domain.Enums;
using UserManagement.Services.DTOs.Responses;
using UserManagement.Services.DTOs.Requests;

namespace UserManagement.Services
{
    public class IdentityService : IIdentityService
    {
        private readonly UserManager<User> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly JwtSettings _jwtSettings;
        private readonly TokenValidationParameters _tokenValidationParameters;
        private readonly IRefreshTokenRepository _refreshTokenRepository;

        public IdentityService(UserManager<User> userManager, RoleManager<IdentityRole> roleManager,JwtSettings jwtSettings, 
                                TokenValidationParameters tokenValidationParameters, 
                                IRefreshTokenRepository refreshTokenRepository)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _jwtSettings = jwtSettings;
            _tokenValidationParameters = tokenValidationParameters;
            _refreshTokenRepository = refreshTokenRepository;
        }

        public async Task<AuthenticationResponse> LoginAsync(UserAuthenticationRequest userReq)
        {

            var user = await _userManager.FindByEmailAsync(userReq.Email);

            if (user == null)
            {
                return new AuthenticationResponse
                {
                    ErrorMessages = new[] { "User does not exist." },
                    Success = false
                };
            }

            var userHasValidPassword = await _userManager.CheckPasswordAsync(user, userReq.Password);
            
            await _userManager.AddClaimAsync(user, new Claim(Claims.Users, "true"));//TODO:DELETE
            
            if (!userHasValidPassword)
            {
                return new AuthenticationResponse
                {
                    ErrorMessages = new[] { "Email/Password are incorrect." },
                    Success = false
                };
            }

            return await GenerateAuthenticationResultAsync(user);
        }

        public async Task<AuthenticationResponse> RegisterAsync(UserAuthenticationRequest userReq)
        {
            var existingUser = await _userManager.FindByEmailAsync(userReq.Email);

            if (existingUser != null)
            {
                return new AuthenticationResponse
                {
                    ErrorMessages = new[] { "User with this email already exists." },
                    Success = false
                };
            }

            var newUser = new User
            {
                Id = Guid.NewGuid().ToString(),
                Email = userReq.Email,
                UserName = userReq.UserName,
                LastName = userReq.LastName
            };

            var createdUser = await _userManager.CreateAsync(newUser, userReq.Password);

            if (!createdUser.Succeeded)
            {
                return new AuthenticationResponse
                {
                    ErrorMessages = createdUser.Errors.Select(x => x.Description),
                    Success = false
                };
            }

            return await GenerateAuthenticationResultAsync(newUser);
        }

        public async Task<AuthenticationResponse> RefreshTokenAsync(RefreshTokenRequest refTokenReq)
        {
            var validatedToken = GetPrincipalFromToken(refTokenReq.Token);

            if(validatedToken == null)
            {
                return new AuthenticationResponse
                { 
                    ErrorMessages = new[] { "Invalid token."} 
                };
            }

            var expiryDateUnix = long.Parse(validatedToken.Claims.Single(x => x.Type == JwtRegisteredClaimNames.Exp).Value);

            var expiryDateTimeUtc = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc)
                .AddSeconds(expiryDateUnix);

            if(expiryDateTimeUtc > DateTime.UtcNow)
            {
                return new AuthenticationResponse { ErrorMessages = new[] { "This token hasn't expired yet" } };
            }

            var jti = validatedToken.Claims.Single(x => x.Type == JwtRegisteredClaimNames.Jti).Value;

            var storedRefreshToken = await _refreshTokenRepository.ReadRefreshToken(refTokenReq.RefreshToken);

            if(storedRefreshToken ==null)
            {
                return new AuthenticationResponse { ErrorMessages = new[] { "The refresh token does not exist." } };
            }

            if (DateTime.UtcNow > storedRefreshToken.ExpiryDate)
            {
                return new AuthenticationResponse { ErrorMessages = new[] { "The refresh token has expired." } };
            }

            if (storedRefreshToken.Invalidated)
            {
                return new AuthenticationResponse { ErrorMessages = new[] { "The refresh token has been invalidated." } };
            }

            if (storedRefreshToken.Used)
            {
                return new AuthenticationResponse { ErrorMessages = new[] { "The refresh token has been used already." } };
            }

            if (storedRefreshToken.JwtId != jti)
            {
                return new AuthenticationResponse { ErrorMessages = new[] { "The refresh token does not match the JWT." } };
            }

            storedRefreshToken.Used = true;
            await _refreshTokenRepository.UpdateRefreshToken(storedRefreshToken);

            var user = await _userManager.FindByIdAsync(validatedToken.Claims.Single(x => x.Type == "id").Value);
            return await GenerateAuthenticationResultAsync(user);

        }

        private ClaimsPrincipal GetPrincipalFromToken(string token)
        {
            var tokenHandler = new JwtSecurityTokenHandler();

            try
            {
                var principal = tokenHandler.ValidateToken(token, _tokenValidationParameters, out var validatedToken);
                if(!IsJwtWithValidSecurityAlgorithm(validatedToken))
                {
                    return null;
                }
                return principal;
            }
            catch
            {
                return null;
            }
        }

        private bool IsJwtWithValidSecurityAlgorithm(SecurityToken validatedToken)
        {
            return (validatedToken is JwtSecurityToken jwtSecurityToken) &&
                jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, StringComparison.InvariantCultureIgnoreCase);
        }


        private async Task<AuthenticationResponse> GenerateAuthenticationResultAsync(User user)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_jwtSettings.Secret);

            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.Email),
                new Claim(JwtRegisteredClaimNames.Email, user.Email),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim("id", user.Id)
            };

            var userClaims = await _userManager.GetClaimsAsync(user);
            claims.AddRange(userClaims);

            var userRoles = await _userManager.GetRolesAsync(user);
            foreach (var userRole in userRoles)
            {
                claims.Add(new Claim(ClaimTypes.Role, userRole));
                var role = await _roleManager.FindByNameAsync(userRole);
                if (role == null) continue;
                var roleClaims = await _roleManager.GetClaimsAsync(role);

                foreach (var roleClaim in roleClaims)
                {
                    if (!claims.Contains(roleClaim))
                        claims.Add(roleClaim);            
                }
            }

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.Add(_jwtSettings.TokenLifeTime),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);

            var refreshToken = new RefreshToken
            {
                JwtId = token.Id,
                UserId = user.Id,
                CreationDate = DateTime.UtcNow,
                ExpiryDate = DateTime.UtcNow.AddMonths(2)
            };

            var saved = await _refreshTokenRepository.CreateRefreshToken(refreshToken);

            if(!saved)
            {
                return new AuthenticationResponse
                {
                    Success = false,
                    Token = null,
                    RefreshToken = null,
                    ErrorMessages = new[] { "Refresh token cannot be created correctly." }
                };
            }

            return new AuthenticationResponse
            {
                Success = true,
                Token = tokenHandler.WriteToken(token),
                RefreshToken = refreshToken.Token
            };
        }
    }
}
