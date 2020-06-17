using API.UserManagement.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.UserManagement.Services
{
    public interface IUsersService
    {
        List<User> GetUsers();
        User GetUserById(Guid id);
    }
}
