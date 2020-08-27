using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UserManagement.API.Controllers.v1.Contracts.Requests;

namespace UserManagement.API.Controllers.v1.RequestsValidators
{
    public class UserRegistrationRequestValidator : AbstractValidator<UserRegistrationRequest>
    {
        public UserRegistrationRequestValidator()
        {
            RuleFor(x => x.Email).NotEmpty().Must(x => x.Contains("@"));
            RuleFor(x => x.UserName).NotEmpty().MaximumLength(15).MinimumLength(3).Matches("^[a-zA-Z0-9 ]*$");
            RuleFor(x => x.LastName).NotEmpty().MaximumLength(15).MinimumLength(3).Matches("^[a-zA-Z0-9 ]*$");
            RuleFor(x => x.Password).NotEmpty().MaximumLength(15).MinimumLength(3).Matches("^[a-zA-Z0-9 ]*$");
        }
    }
}
