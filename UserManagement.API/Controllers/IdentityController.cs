using UserManagement.Services.Boundaries;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UserManagement.Services.DTOs.Requests;
using UserManagement.API.Controllers.v1.Contracts.Requests;
using UserManagement.API.Controllers.v1.Contracts.Responses;
using UserManagement.API.Controllers.v1.Contracts;
using AutoMapper;

namespace UserManagement.API.Controllers
{
    public class IdentityController: Controller
    {
        private readonly IIdentityService _identityService;
        private readonly IMapper _mapper;

        public IdentityController(IIdentityService identityService, IMapper mapper)
        {
            _identityService = identityService;
            _mapper = mapper;
        }

        [HttpPost(ApiRoutes.Identity.Register)]
        public async Task<IActionResult> Register([FromBody] UserRegistrationRequest request)
        {
            var serviceReq = _mapper.Map<UserAuthenticationRequest>(request);
            var authResponse = await _identityService.RegisterAsync(serviceReq);

            if(!authResponse.Success)
            {
                return BadRequest(new AuthFailureResponse
                {
                    ErrorMessage = authResponse.ErrorMessages
                });
            }

            return Ok(new ApiResponse<AuthSuccessResponse>( 
                new AuthSuccessResponse
                {
                    Token = authResponse.Token,
                    RefreshToken = authResponse.RefreshToken
                })
            );
        }

        [HttpPost(ApiRoutes.Identity.Login)]
        public async Task<IActionResult> Login([FromBody] UserLoginRequest request)
        {
            if(!ModelState.IsValid)
            {
                return BadRequest(new AuthFailureResponse
                {
                    ErrorMessage = ModelState.Values.SelectMany(x => x.Errors.Select(y => y.ErrorMessage))
                });
            }
            var serviceReq = _mapper.Map<UserAuthenticationRequest>(request);
            var authResponse = await _identityService.LoginAsync(serviceReq);

            if (!authResponse.Success)
            {
                return BadRequest(new AuthFailureResponse
                {
                    ErrorMessage = authResponse.ErrorMessages
                });
            }

            return Ok(new ApiResponse<AuthSuccessResponse>(
                new AuthSuccessResponse
                {
                    Token = authResponse.Token,
                    RefreshToken = authResponse.RefreshToken
                })
            );
        }

        [HttpPost(ApiRoutes.Identity.Refresh)]
        public async Task<IActionResult> Refresh([FromBody] v1.Contracts.Requests.RefreshTokenRequest request)
        {
            var serviceReq = _mapper.Map<UserManagement.Services.DTOs.Requests.RefreshTokenRequest>(request);
            var authResponse = await _identityService.RefreshTokenAsync(serviceReq);

            if (!authResponse.Success)
            {
                return BadRequest(new AuthFailureResponse
                {
                    ErrorMessage = authResponse.ErrorMessages
                });
            }

            return Ok(new ApiResponse<AuthSuccessResponse>(
                new AuthSuccessResponse
                {
                    Token = authResponse.Token,
                    RefreshToken = authResponse.RefreshToken
                })
            );

        }

    }
}
