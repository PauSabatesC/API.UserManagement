using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace API.UserManagement.Domain
{
    public class AdminActions
    {
        [Key]
        public string Id { get; set; }

        public string Action { get; set; }

        public DateTime Date { get; set; }

        public string UserAdminId { get; set; }

        [ForeignKey(nameof(UserAdminId))]
        public User User { get; set; }
    }
}
