using API.LoginAndRegister.Contracts.v1;
using API.UserManagement.Contracts.v1.Requests;
using API.UserManagement.Contracts.v1.Responses;
using API.UserManagement.Domain;
using API.UserManagement.Services;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.UserManagement.Controllers.v1
{
    public class UsersController : Controller
    {
        private readonly IUsersService _usersService;

        public UsersController(IUsersService usersService)
        {
            _usersService = usersService;
        }


        [HttpGet(ApiRoutes.Users.GetAll)]
        public IActionResult GetAll()
        {
            return Ok(_usersService.GetUsers());
        }

        [HttpGet(ApiRoutes.Users.Get)]
        public IActionResult Get([FromRoute] Guid userId)
        {
            var user = _usersService.GetUserById(userId);
            if ( user == null) return NotFound();
            else return Ok(user);
        }

        [HttpPost(ApiRoutes.Users.Create)]
        public IActionResult Create([FromBody] CreateUserRequest user)
        {
            User _user = new User { Name = user.Name, Id = Guid.NewGuid() };
            //_users.Add(_user);

            var baseUrl = $"{HttpContext.Request.Scheme}://{HttpContext.Request.Host.ToUriComponent()}";
            var locationUri = baseUrl + "/" + ApiRoutes.Users.Get.Replace("{userId}", _user.Id.ToString());

            var response = new PostResponse { Id = _user.Id, Name = _user.Name };
            return Created(locationUri, response);
        }

        [HttpPut(ApiRoutes.Users.Update)]
        public IActionResult Update([FromRoute] Guid userId, [FromBody] UpdateUserRequest request)
        {
            User user = new User{Id = userId, Name = request.Name};

            var updated = _usersService.UpdateUser(user);
            if ( !updated) return NotFound();
            else return Ok(user);
        }

        [HttpDelete(ApiRoutes.Users.Delete)]
        public IActionResult Update([FromRoute] Guid userId)
        {
            var deleted = _usersService.DeleteUser(userId);
            if ( !deleted) return NotFound();
            else return NoContent();
        }
    }
}
