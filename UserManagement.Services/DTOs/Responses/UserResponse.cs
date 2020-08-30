using System;
using System.Collections.Generic;
using System.Text;

namespace UserManagement.Services.DTOs.Responses
{
    public class UserResponse
    {
        public string LastName { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public string UpdatedBy { get; set; }
        public string CreatedBy { get; set; }

    }
}
