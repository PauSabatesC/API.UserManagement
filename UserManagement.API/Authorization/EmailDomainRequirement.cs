using Microsoft.AspNetCore.Authorization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace UserManagement.API.Authorization
{
    public class EmailDomainRequirement : IAuthorizationRequirement
    {
        public string DomainName { get; }

        public EmailDomainRequirement(string domainName)
        {
            DomainName = domainName;
        }
    }
}
