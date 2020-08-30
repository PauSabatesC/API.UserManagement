using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UserManagement.API.Controllers.v1.Contracts.Requests.Queries;
using UserManagement.API.Controllers.v1.Contracts.Responses;
using UserManagement.API.Services;
using UserManagement.Services.DTOs.Requests;
using UserManagement.Services.DTOs.Responses;

namespace UserManagement.API.Common.Utils
{
    public static class PaginationUtils
    {
        public static PagedResponse<T> BuildPaginatedResponse<T>(IUriService<IPagination> uriService, IPagination paginationQuery, IEnumerable<T> data)
        {
            var nextPage = paginationQuery.PageNumber >= 1
                ? uriService.GetAllUsersUri(new PaginationQuery(paginationQuery.PageNumber + 1, paginationQuery.PageSize)).ToString()
                : null;

            var previousPage = paginationQuery.PageNumber - 1 >= 1
                ? uriService.GetAllUsersUri(new PaginationQuery(paginationQuery.PageNumber - 1, paginationQuery.PageSize)).ToString()
                : null;

            var pagedResponse = new PagedResponse<T>
            {
                Data = data,
                PageNumber = paginationQuery.PageNumber >= 1 ? paginationQuery.PageNumber : (int?)null,
                PageSize = paginationQuery.PageSize >= 1 ? paginationQuery.PageNumber : (int?)null,
                NextPage = data.Any() ? nextPage : null,
                PreviousPage = previousPage
            };

            return pagedResponse;
        }
    }
}
