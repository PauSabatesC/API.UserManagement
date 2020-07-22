using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace API.UserManagement.Domain
{
    public class UserMetaData
    {
        [Key]
        public string Id { get; set; }
        public int TimesPasswordReset { get; set; }

        public List<Historic> LoginList { get; set; }

        //public string CountriesLogin { get; set; }

        //public string BrowsersUsedLogin { get; set; }

        public string UserId { get; set; }

        [ForeignKey(nameof(UserId))]
        public User User { get; set; }
    }
}
