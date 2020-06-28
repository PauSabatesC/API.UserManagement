using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace API.UserManagement.Domain
{
    public class User : BaseEntity
    {
        [Column(TypeName = "varchar(150)")]
        public string LastName { get; set; }
    }
}
