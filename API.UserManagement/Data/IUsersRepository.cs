using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using API.UserManagement.Domain;
    
namespace API.UserManagement.Data
{
    public interface IUsersRepository
    {
        Task<bool> CreateUser();
        Task<IEnumerable<User>> ReadUsers();
        Task<User> ReadUser(Guid id);
        Task<bool> UpdateUser(User user);
        Task<bool> DeleteUser(Guid id);

    }
}