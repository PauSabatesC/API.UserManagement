using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace API.UserManagement.Domain
{
    public class User
    {
        [Key]
        public Guid Id { get; set; }
        public string Name { get; set; }
    }
}
