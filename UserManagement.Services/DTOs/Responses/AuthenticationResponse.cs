using System.Collections.Generic;

namespace UserManagement.Services.DTOs.Responses
{
    public class AuthenticationResponse
    {
        public string Token { get; set; }
        public string RefreshToken { get; set; }
        public bool Success { get; set; }
        public IEnumerable<string> ErrorMessages { get; set; }

    }
}
