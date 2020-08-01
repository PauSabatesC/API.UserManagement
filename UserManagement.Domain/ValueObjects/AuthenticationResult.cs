using System.Collections.Generic;

namespace UserManagement.Domain.ValueObjects
{
    public class AuthenticationResult : ValueObject
    {
        public string Token { get; set; }
        public string RefreshToken { get; set; }
        public bool Success { get; set; }
        public IEnumerable<string> ErrorMessages { get; set; }

        protected override IEnumerable<object> GetAtomicValues()
        {
            yield return Token;
            yield return RefreshToken;
            yield return Success;
            yield return ErrorMessages;
        }
    }
}
