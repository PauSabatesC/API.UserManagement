using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using API.UserManagement.Domain;
    
namespace API.UserManagement.Data
{
    public interface IUsersRepository
    {
        Task<User> CreateUser(User user);
        Task<IEnumerable<User>> ReadUsers();
        Task<User> ReadUser(string id);
        Task<bool> UpdateUser(User user);
        Task<bool> DeleteUser(string id);

    }
}