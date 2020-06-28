using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.UserManagement.Domain
{
    public class BaseEntity : IdentityUser
    {
        public DateTime AddedDate { get; set; }
        public DateTime ModifiedDate { get; set; }
    }
}
