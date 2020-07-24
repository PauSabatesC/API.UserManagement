using System.Collections.Generic;
using System.Threading.Tasks;
using UserManagement.Domain.Entities;

namespace UserManagement.Domain.RepositoryInterfaces
{
    public interface IUsersAdminManagementRepository
    {
        Task<User> CreateUser(User user);
        Task<IEnumerable<User>> ReadUsers();
        Task<User> ReadUser(string id);
        Task<bool> UpdateUser(User user);
        Task<bool> DeleteUser(string id);

    }
}