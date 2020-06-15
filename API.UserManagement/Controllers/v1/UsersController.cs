using API.LoginAndRegister.Contracts.v1;
using API.UserManagement.Domain;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.UserManagement.Controllers.v1
{
    public class UsersController : Controller
    {
        List<User> _users = new List<User>();

        [HttpGet(ApiRoutes.Users.GetAll)]
        public IActionResult GetAll()
        {
            return Ok(new { Name = "Pau" });
        }

        [HttpPost(ApiRoutes.Users.Create)]
        public IActionResult Create([FromBody] User user)
        {            
            _users.Add(user);

            var baseUrl = $"{HttpContext.Request.Scheme}://{HttpContext.Request.Host.ToUriComponent()}";
            var locationUri = baseUrl + "/" + ApiRoutes.Users.Get.Replace("{userId}", user.Id.ToString());

            return Created(locationUri, user);
        }
    }
}
