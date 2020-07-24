using System.ComponentModel.DataAnnotations.Schema;

namespace UserManagement.Domain.Entities
{
    public class User : UserBaseEntity
    {
        [Column(TypeName = "varchar(150)")]
        public string LastName { get; set; }
        public override string UserName { get => base.UserName; set => base.UserName = value; }
        public override string PasswordHash { get => base.PasswordHash; set => base.PasswordHash = value; }
        public override string Email { get => base.Email; set => base.Email = value; }
        public string UpdatedBy { get; set; }
        public string CreatedBy { get; set; }

        [ForeignKey(nameof(UpdatedBy))]
        public User AdminUpdater { get; set; }

        [ForeignKey(nameof(CreatedBy))]
        public User AdminCreator { get; set; }

    }
}
