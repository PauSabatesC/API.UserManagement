using API.UserManagement.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.UserManagement.Services
{
    public class UserService : IUsersService
    {
        private readonly List<User> _users = new List<User>();

        public UserService()
        {
            for (int i = 0; i < 5; i++)
            {
                _users.Add(new User { Id = Guid.NewGuid(), Name = "user-" + i });
            }
        }

        public User GetUserById(Guid id)
        {
            return _users.FirstOrDefault<User>(x => x.Id == id);
        }

        public List<User> GetUsers()
        {
            return _users;
        }
    }
}
