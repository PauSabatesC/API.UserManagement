using System;
using System.Collections.Generic;
using System.Text;

namespace UserManagement.Services.DTOs.Requests
{
    public class RefreshTokenRequest
    {
        public string Token { get; set; }
        public string RefreshToken { get; set; }

    }
}
