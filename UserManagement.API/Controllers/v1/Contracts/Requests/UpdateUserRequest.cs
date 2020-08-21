using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace UserManagement.API.Controllers.v1.Contracts.Requests
{
    public class UpdateUserRequest
    {
        public string UserName { get; set; }
    }
}