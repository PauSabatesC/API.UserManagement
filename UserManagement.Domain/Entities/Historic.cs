using System;
using System.ComponentModel.DataAnnotations;

namespace UserManagement.Domain.Entities
{
    public class Historic
    {
        [Key]
        public Guid Id { get; set; }
        public DateTime Date { get; set; }
    }
}
