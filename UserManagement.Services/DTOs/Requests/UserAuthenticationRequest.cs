using System;
using System.Collections.Generic;
using System.Text;

namespace UserManagement.Services.DTOs.Requests
{
    public class UserAuthenticationRequest
    {
        public string Email { get; set; }
        public string Password { get; set; }
        public string UserName { get; set; }
        public string LastName { get; set; }

    }
}
