using API.LoginAndRegister.Contracts.v1;
using API.UserManagement.Contracts.v1.Requests;
using API.UserManagement.Contracts.v1.Responses;
using API.UserManagement.Services;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.UserManagement.Controllers.v1
{
    public class IdentityController: Controller
    {
        private readonly IIdentityService _identityService;

        public IdentityController(IIdentityService identityService)
        {
            _identityService = identityService;
        }

        [HttpPost(ApiRoutes.Identity.Register)]
        public async Task<IActionResult> Register([FromBody] UserRegistrationRequest request)
        {
            var authResponse = await _identityService.RegisterAsync(request.email, request.password);

            if(!authResponse.Success)
            {
                return BadRequest(new AuthFailureResponse
                {
                    ErrorMessage = authResponse.ErrorMessages
                });
            }

            return Ok(new AuthSuccessResponse
            {
                Token = authResponse.Token
            });
        }

    }
}
