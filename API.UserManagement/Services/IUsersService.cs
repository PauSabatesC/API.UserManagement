using API.UserManagement.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.UserManagement.Services
{
    public interface IUsersService
    {
        Task<IEnumerable<User>> GetUsers();
        Task<User> GetUserById(string id);
        Task<bool> UpdateUser(User user);
        Task<bool> DeleteUser(string id);
        Task<User> CreateUser(User user);
    }
}
