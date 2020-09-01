using UserManagement.Domain.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;
using UserManagement.Services.DTOs.Responses;
using UserManagement.Services.DTOs.Requests;

namespace UserManagement.Services.Boundaries
{
    public interface IUsersService
    {
        Task<IEnumerable<UserResponse>> GetUsers(PaginationRequest paginationReq, GetAllPostsRequestFilter usersFilter = null);
        Task<User> GetUserById(string id);
        Task<bool> UpdateUser(User user);
        Task<bool> DeleteUser(string id);
        Task<User> CreateUser(User user, string adminId);
        Task<bool> GiveUserAdminClaims(string id);
    }
}
