using Microsoft.AspNetCore.Identity;
using System;

namespace UserManagement.Domain.Entities
{
    public class UserBaseEntity : IdentityUser
    {
        public DateTime AddedDate { get; set; }
        public DateTime ModifiedDate { get; set; }
    }
}
