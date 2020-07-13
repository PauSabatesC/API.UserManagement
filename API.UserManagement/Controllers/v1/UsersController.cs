using API.LoginAndRegister.Contracts.v1;
using API.UserManagement.Contracts.v1.Requests;
using API.UserManagement.Contracts.v1.Responses;
using API.UserManagement.Domain;
using API.UserManagement.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.UserManagement.Controllers.v1
{
    [Authorize(AuthenticationSchemes =JwtBearerDefaults.AuthenticationScheme)]
    public class UsersController : Controller
    {
        private readonly IUsersService _usersService;

        public UsersController(IUsersService usersService)
        {
            _usersService = usersService;
        }


        [HttpGet(ApiRoutes.Users.GetAll)]
        public async Task<IActionResult> GetAll()
        {
            var users = await _usersService.GetUsers();
            return Ok(users);
        }

        [HttpGet(ApiRoutes.Users.Get)]
        public async Task<IActionResult> Get([FromRoute] string userId)
        {
            var user = await _usersService.GetUserById(userId);
            if ( user == null) return NotFound();
            else return Ok(user);
        }

        [HttpPost(ApiRoutes.Users.Create)]
        public async Task<IActionResult> Create([FromBody] CreateUserRequest user)
        {
            User aux_user = new User { UserName = user.Name };


            var userCreated = await _usersService.CreateUser(aux_user);

            var baseUrl = $"{HttpContext.Request.Scheme}://{HttpContext.Request.Host.ToUriComponent()}";
            var locationUri = baseUrl + "/" + ApiRoutes.Users.Get.Replace("{userId}", userCreated.Id.ToString());

            var response = new PostResponse { Id = userCreated.Id, UserName = userCreated.UserName };
            return Created(locationUri, response);
        }

        [HttpPut(ApiRoutes.Users.Update)]
        public async Task<IActionResult> Update([FromRoute] string userId, [FromBody] UpdateUserRequest request)
        {
            var user = await _usersService.GetUserById(userId);
            user.UserName = request.UserName;

            var updated = await _usersService.UpdateUser(user);
            if ( !updated) return NotFound();
            else return Ok(user);
        }

        [HttpDelete(ApiRoutes.Users.Delete)]
        public async Task<IActionResult> Delete([FromRoute] string userId)
        {
            var deleted = await _usersService.DeleteUser(userId);
            if ( !deleted) return NotFound();
            else return NoContent();
        }
    }
}
