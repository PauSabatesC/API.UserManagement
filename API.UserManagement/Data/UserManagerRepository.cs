using API.UserManagement.Domain;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.UserManagement.Data
{
    public class UserManagerRepository: IUserRepository
    {
        private readonly UserManager<User> _userManager;

        public UserManagerRepository(UserManager<User> userManager)
        {

        }
    }
}
