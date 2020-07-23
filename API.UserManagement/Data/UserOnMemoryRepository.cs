using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.UserManagement.Domain;
using Microsoft.EntityFrameworkCore;

namespace API.UserManagement.Data
{
    public class UserOnMemoryRepository : IUsersAdminManagementRepository
    {
        private readonly List<User> _users = new List<User>();

        public UserOnMemoryRepository()
        {
            for (int i = 0; i < 5; i++)
            {
                _users.Add(new User { Id = Guid.NewGuid().ToString(), UserName = "user-" + i });
            }
        }

        public async Task<User> CreateUser(User user)
        {
            user.Id = new Guid().ToString();
            await Task.Run(() => _users.Add(user));
            return user;
        }

        public async Task<bool> DeleteUser(string id)
        {
            int index = await Task.Run(() => _users.FindIndex(x => x.Id == id));
            if(index == -1) return false;
            else
            {
                await Task.Run(() => _users.RemoveAt(index));
                return true;
            }
        }

        public async Task<User> ReadUser(string id)
        {
            return await Task.Run(() => 
                _users.FirstOrDefault<User>(x => x.Id == id)
            );
        }

        public async Task<IEnumerable<User>> ReadUsers()
        {
            return await Task.Run(() => _users);
        }

        public async Task<bool> UpdateUser(User user)
        {
            int index = await Task.Run(() => _users.FindIndex(x => x.Id == user.Id));
            if(index == -1) return false;
            else
            {
                await Task.Run(() =>_users[index] = user);
                return true;
            }
        }
    }
}