using UserManagement.Domain.RepositoryInterfaces;
using UserManagement.Domain.Entities;
using Microsoft.AspNetCore.Identity;

namespace UserManagement.Infrastructure.Database
{
    public class UserManagerRepository: IUserRepository
    {
        private readonly UserManager<User> _userManager;

        public UserManagerRepository(UserManager<User> userManager)
        {

        }
    }
}
