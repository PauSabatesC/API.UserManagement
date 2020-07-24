using UserManagement.Domain.RepositoryInterfaces;
using UserManagement.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UserManagement.Services.Boundaries;

namespace UserManagement.Services
{
    public class UsersService : IUsersService
    {
        private readonly IUsersAdminManagementRepository _usersRepository;

        public UsersService(IUsersAdminManagementRepository usersRepository)
        {
            _usersRepository = usersRepository;
        }

        public async Task<User> GetUserById(string id)
        {
            return await _usersRepository.ReadUser(id);
        }

        public async Task<IEnumerable<User>> GetUsers()
        {
            return await _usersRepository.ReadUsers();
        }

        public async Task<bool> UpdateUser(User user)
        {
            user.ModifiedDate = DateTime.Now;

            return await _usersRepository.UpdateUser(user);
        }

        public async Task<bool> DeleteUser(string id)
        {
            return await _usersRepository.DeleteUser(id);
        }

        public async Task<User> CreateUser(User user, string adminId)
        {
            user.AddedDate = DateTime.Now;
            
            var admin = await _usersRepository.ReadUser(adminId);
            user.AdminCreator = admin;
            
            await _usersRepository.CreateUser(user);
            return user;
        }
    }
}
