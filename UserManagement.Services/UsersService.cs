using UserManagement.Domain.RepositoryInterfaces;
using UserManagement.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UserManagement.Services.Boundaries;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;
using UserManagement.Domain.Enums;

namespace UserManagement.Services
{
    public class UsersService : IUsersService
    {
        private readonly IUsersAdminManagementRepository _usersRepository;
        private readonly UserManager<User> _userManager;

        public UsersService(IUsersAdminManagementRepository usersRepository, UserManager<User> userManager)
        {
            _usersRepository = usersRepository;
            _userManager = userManager;
        }

        public async Task<User> GetUserById(string id)
        {
            return await _usersRepository.ReadUserAsync(id);
        }

        public async Task<IEnumerable<User>> GetUsers()
        {
            return await _usersRepository.ReadUsersAsync();
        }

        public async Task<bool> UpdateUser(User user)
        {
            user.ModifiedDate = DateTime.Now;

            return await _usersRepository.UpdateUserAsync(user);
        }

        public async Task<bool> DeleteUser(string id)
        {
            return await _usersRepository.DeleteUserAsync(id);
        }

        public async Task<User> CreateUser(User user, string adminId)
        {
            user.AddedDate = DateTime.Now;
            
            var admin = await _usersRepository.ReadUserAsync(adminId);
            user.AdminCreator = admin;
            
            await _usersRepository.CreateUserAsync(user);
            return user;
        }

        public async Task<bool> GiveUserAdminClaims(string id)
        {
            var user = await _usersRepository.ReadUserAsync(id);

            if(user == null)
            {
                return false;
            }

            var usersClaim = await _userManager.AddClaimAsync(user, new Claim(Claims.Users, "true"));
            
            if(!usersClaim.Succeeded)
            {
                return false;
            }

            return true;
        }
    }
}
