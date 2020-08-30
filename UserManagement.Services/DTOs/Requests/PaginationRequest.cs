using System;
using System.Collections.Generic;
using System.Text;

namespace UserManagement.Services.DTOs.Requests
{
    public class PaginationRequest : IPagination
    {
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
    }

    public interface IPagination 
    {
         int PageNumber { get; set; }
         int PageSize { get; set; }
    }

}
