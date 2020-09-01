using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace UserManagement.API.Controllers.v1.Contracts.Requests.Queries
{
    public class GetAllUsersQuery
    {
        public string UserId { get; set; }
    }
}
