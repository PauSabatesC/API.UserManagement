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
using UserManagement.Domain.ValueObjects;
using System.Collections.Generic;
using UserManagement.Domain.Enums;

namespace UserManagement.Services
{
    public class IdentityService : IIdentityService
    {
        private readonly UserManager<User> _userManager;
        private readonly JwtSettings _jwtSettings;
        private readonly TokenValidationParameters _tokenValidationParameters;
        private readonly IRefreshTokenRepository _refreshTokenRepository;

        public IdentityService(UserManager<User> userManager, JwtSettings jwtSettings, 
                                TokenValidationParameters tokenValidationParameters, 
                                IRefreshTokenRepository refreshTokenRepository)
        {
            _userManager = userManager;
            _jwtSettings = jwtSettings;
            _tokenValidationParameters = tokenValidationParameters;
            _refreshTokenRepository = refreshTokenRepository;
        }

        public async Task<AuthenticationResult> LoginAsync(string email, string password)
        {
            var user = await _userManager.FindByEmailAsync(email);

            if (user == null)
            {
                return new AuthenticationResult
                {
                    ErrorMessages = new[] { "User does not exist." },
                    Success = false
                };
            }

            var userHasValidPassword = await _userManager.CheckPasswordAsync(user, password);
            
            await _userManager.AddClaimAsync(user, new Claim(ClaimsEnum.Users, "true"));//TODO:DELETE
            
            if (!userHasValidPassword)
            {
                return new AuthenticationResult
                {
                    ErrorMessages = new[] { "Email/Password are incorrect." },
                    Success = false
                };
            }

            return await GenerateAuthenticationResultAsync(user);
        }

        public async Task<AuthenticationResult> RegisterAsync(string email, string password)
        {
            var existingUser = await _userManager.FindByEmailAsync(email);

            if (existingUser != null)
            {
                return new AuthenticationResult
                {
                    ErrorMessages = new[] { "User with this email already exists." },
                    Success = false
                };
            }

            var userId = Guid.NewGuid();
            var newUser = new User
            {
                Id = userId.ToString(),
                Email = email,
                UserName = email
            };

            var createdUser = await _userManager.CreateAsync(newUser, password);

            if (!createdUser.Succeeded)
            {
                return new AuthenticationResult
                {
                    ErrorMessages = createdUser.Errors.Select(x => x.Description),
                    Success = false
                };
            }

            return await GenerateAuthenticationResultAsync(newUser);
        }

        public async Task<AuthenticationResult> RefreshTokenAsync(string token, string refreshToken)
        {
            var validatedToken = GetPrincipalFromToken(token);

            if(validatedToken == null)
            {
                return new AuthenticationResult
                { 
                    ErrorMessages = new[] { "Invalid token."} 
                };
            }

            var expiryDateUnix = long.Parse(validatedToken.Claims.Single(x => x.Type == JwtRegisteredClaimNames.Exp).Value);

            var expiryDateTimeUtc = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc)
                .AddSeconds(expiryDateUnix);

            if(expiryDateTimeUtc > DateTime.UtcNow)
            {
                return new AuthenticationResult { ErrorMessages = new[] { "This token hasn't expired yet" } };
            }

            var jti = validatedToken.Claims.Single(x => x.Type == JwtRegisteredClaimNames.Jti).Value;

            var storedRefreshToken = await _refreshTokenRepository.ReadRefreshToken(refreshToken);

            if(storedRefreshToken ==null)
            {
                return new AuthenticationResult { ErrorMessages = new[] { "The refresh token does not exist." } };
            }

            if (DateTime.UtcNow > storedRefreshToken.ExpiryDate)
            {
                return new AuthenticationResult { ErrorMessages = new[] { "The refresh token has expired." } };
            }

            if (storedRefreshToken.Invalidated)
            {
                return new AuthenticationResult { ErrorMessages = new[] { "The refresh token has been invalidated." } };
            }

            if (storedRefreshToken.Used)
            {
                return new AuthenticationResult { ErrorMessages = new[] { "The refresh token has been used already." } };
            }

            if (storedRefreshToken.JwtId != jti)
            {
                return new AuthenticationResult { ErrorMessages = new[] { "The refresh token does not match the JWT." } };
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


        private async Task<AuthenticationResult> GenerateAuthenticationResultAsync(User user)
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

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new System.Security.Claims.ClaimsIdentity(claims),
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
                return new AuthenticationResult
                {
                    Success = false,
                    Token = null,
                    RefreshToken = null,
                    ErrorMessages = new[] { "Refresh token cannot be created correctly." }
                };
            }

            return new AuthenticationResult
            {
                Success = true,
                Token = tokenHandler.WriteToken(token),
                RefreshToken = refreshToken.Token
            };
        }
    }
}
