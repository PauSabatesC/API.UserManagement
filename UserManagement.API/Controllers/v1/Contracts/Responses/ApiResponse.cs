using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace UserManagement.API.Controllers.v1.Contracts.Responses
{
    public class ApiResponse<T>
    {
        public ApiResponse() { }
        public ApiResponse(T response)
        {
            Data = response;
        }

        public T Data { get; set; }
    }
}
