using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.UserManagement.Domain
{
    public class User
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
    }
}
