using System;
using System.Collections.Generic;
using System.Text;
using UserManagement.API.Controllers.v1.Contracts.Requests.Queries;
using UserManagement.Services.DTOs.Requests;

namespace UserManagement.API.Services
{
    public interface IUriService<T>
    {
        Uri GetUserUri(string userId);
        Uri GetAllUsersUri(T paginationRequest);
    }
}
