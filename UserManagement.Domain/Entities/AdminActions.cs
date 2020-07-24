using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace UserManagement.Domain.Entities
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
