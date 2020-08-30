using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UserManagement.Services.DTOs.Requests;

namespace UserManagement.API.Controllers.v1.Contracts.Requests.Queries
{
    public class PaginationQuery : IPagination
    {
        public PaginationQuery()
        {
            PageNumber = 1;
            PageSize = 5;
        }
        public PaginationQuery(int pageNumber, int pageSize)
        {
            PageNumber = pageNumber;
            PageSize = pageSize;
            PageSize = pageSize;
        }

        public int PageNumber { get; set; }
        public int PageSize { get; set; }
    }
}
