using UserManagement.Domain.Entities;
using UserManagement.Services.Boundaries;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using UserManagement.API.Extensions;
using UserManagement.Domain.Enums;
using UserManagement.API.Controllers.v1.Contracts.Requests;
using UserManagement.API.Controllers.v1.Contracts;
using UserManagement.API.Controllers.v1.Contracts.Responses;
using UserManagement.API.FiltersMiddleware.AuthenticationMiddlewares;
using UserManagement.Services.DTOs.Responses;
using System.Collections.Generic;
using UserManagement.API.Controllers.v1.Contracts.Requests.Queries;
using AutoMapper;
using UserManagement.Services.DTOs.Requests;
using UserManagement.API.Services;
using System.Linq;
using UserManagement.API.Common.Utils;

namespace UserManagement.API.Controllers.v1
{
    //[ApiKeyAuth]
    public class UsersController : Controller
    {
        private readonly IUsersService _usersService;
        private readonly IMapper _mapper;
        private readonly IUriService<IPagination> _uriService;

        public UsersController(IUsersService usersService, IMapper mapper, IUriService<IPagination> uriService)
        {
            _mapper = mapper;
            _usersService = usersService;
            _uriService = uriService;
        }


        [HttpGet(ApiRoutes.Users.GetAll)]
        //[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<IActionResult> GetAll([FromQuery]PaginationQuery paginationQuery)
        {
            var paginationReq = _mapper.Map<PaginationRequest>(paginationQuery);
            var users = await _usersService.GetUsers(paginationReq);

            if(paginationReq == null || paginationReq.PageNumber < 1 || paginationReq.PageSize < 1)
            {
                return Ok(new PagedResponse<UserResponse>(users));
            }

            var paginatedResponse = PaginationUtils.BuildPaginatedResponse<UserResponse>(_uriService,paginationQuery, users);
            return Ok(paginatedResponse);
        }

        [HttpGet(ApiRoutes.Users.Get)]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme), ApiKeyAuth]
        [Authorize(Roles = Roles.Admin)]
        [Authorize(Policy = Policies.MustBeEnterpriseEmail)]
        public async Task<IActionResult> Get([FromRoute] string userId)
        {
            var user = await _usersService.GetUserById(userId);
            if ( user == null) return NotFound();
            else return Ok(user);
        }

        [HttpPost(ApiRoutes.Users.Create)]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [Authorize(Roles = Roles.Admin)]
        [Authorize(Policy = Policies.MustBeEnterpriseEmail)]
        public async Task<IActionResult> Create([FromBody] CreateUserRequest user)
        {
            User aux_user = new User { UserName = user.Name};
            var adminUserId = HttpContext.GetUserId();

            var userCreated = await _usersService.CreateUser(aux_user, adminUserId);

            var locationUri = _uriService.GetUserUri(userCreated.Id.ToString());
            var response = new PostResponse { Id = userCreated.Id, UserName = userCreated.UserName };
            return Created(locationUri, response);
        }

        [HttpPut(ApiRoutes.Users.Update)]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [Authorize(Roles = Roles.Admin)]
        [Authorize(Policy = Policies.MustBeEnterpriseEmail)]
        public async Task<IActionResult> Update([FromRoute] string userId, [FromBody] UpdateUserRequest request)
        {
            var user = await _usersService.GetUserById(userId);
            user.UserName = request.UserName;

            var updated = await _usersService.UpdateUser(user);
            if (!updated) return NotFound();
            else return Ok(user);
        }

        [HttpDelete(ApiRoutes.Users.Delete)]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [Authorize(Roles = Roles.Admin)]
        [Authorize(Policy = Policies.MustBeEnterpriseEmail)]
        public async Task<IActionResult> Delete([FromRoute] string userId)
        {
            var deleted = await _usersService.DeleteUser(userId);
            if ( !deleted) return NotFound();
            else return NoContent();
        }
    }
}
