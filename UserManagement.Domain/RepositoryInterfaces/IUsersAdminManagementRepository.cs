using System.Collections.Generic;
using System.Threading.Tasks;
using UserManagement.Domain.Entities;

namespace UserManagement.Domain.RepositoryInterfaces
{
    public interface IUsersAdminManagementRepository
    {
        Task<User> CreateUserAsync(User user);
        Task<IEnumerable<User>> ReadUsersAsync();
        Task<User> ReadUserAsync(string id);
        Task<bool> UpdateUserAsync(User user);
        Task<bool> DeleteUserAsync(string id);

    }
}