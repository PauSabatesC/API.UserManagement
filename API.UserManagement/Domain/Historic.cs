using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace API.UserManagement.Domain
{
    public class Historic
    {
        [Key]
        public Guid Id { get; set; }
        public DateTime Date { get; set; }
    }
}
