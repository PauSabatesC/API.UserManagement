using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace UserManagement.Services.Extensions
{
    public static class HttpExtensions
    {
        public static string GetUserId(this HttpContext httpContext)
        {
            if(httpContext.User == null)
            {
                return string.Empty;
            }

            return httpContext.User.Claims.Single(x => x.Type == "id").Value;
        }
    }
}
