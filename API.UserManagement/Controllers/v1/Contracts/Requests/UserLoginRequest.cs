using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace API.UserManagement.Controllers.v1.Contracts.Requests
{
    public class UserLoginRequest
    {
        [EmailAddress]
        public string email { get; set; }
        public string password { get; set; }
    }
}
