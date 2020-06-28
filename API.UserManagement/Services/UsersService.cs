using API.UserManagement.Data;
using API.UserManagement.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.UserManagement.Services
{
    public class UsersService : IUsersService
    {
        private readonly IUsersRepository _usersRepository;

        public UsersService(IUsersRepository usersRepository)
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
            //if(user.Id == null) throw new ArgumentNullException("User id cannot be null");
            return await _usersRepository.UpdateUser(user);
        }

        public async Task<bool> DeleteUser(string id)
        {
            //if(id == null) throw new ArgumentNullException("User id cannot be null");
            return await _usersRepository.DeleteUser(id);
        }

        public async Task<User> CreateUser(User user)
        {
            await _usersRepository.CreateUser(user);
            return user;
        }
    }
}
