using UserManagement.Domain.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace UserManagement.Services.Boundaries
{
    public interface IUsersService
    {
        Task<IEnumerable<User>> GetUsers();
        Task<User> GetUserById(string id);
        Task<bool> UpdateUser(User user);
        Task<bool> DeleteUser(string id);
        Task<User> CreateUser(User user, string adminId);
    }
}
