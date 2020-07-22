using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.UserManagement.Controllers.v1.Contracts.Requests
{
    public class CreateUserRequest
    {
        public string Name { get; set; }
    }
}
