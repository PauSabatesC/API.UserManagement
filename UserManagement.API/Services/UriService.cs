using Microsoft.AspNetCore.WebUtilities;
using System;
using System.Collections.Generic;
using System.Text;
using UserManagement.API.Controllers;
using UserManagement.API.Controllers.v1.Contracts;
using UserManagement.Services.Boundaries;
using UserManagement.Services.DTOs.Requests;

namespace UserManagement.API.Services
{
    public class UriService<T> : IUriService<T> where T : IPagination
    {
        public readonly string _baseUri;

        public UriService(string baseUri)
        {
            _baseUri = baseUri;
        }
        
        
        public Uri GetAllUsersUri(T paginationRequest)
        {
            var uri = new Uri(_baseUri);

            if (paginationRequest == null)
            {
                return uri;
            }

            var modifiedUri = QueryHelpers.AddQueryString(_baseUri, "pageNumber", paginationRequest.PageNumber.ToString());
            modifiedUri = QueryHelpers.AddQueryString(modifiedUri, "pageSize", paginationRequest.PageSize.ToString());

            return new Uri(modifiedUri);
        }

        public Uri GetUserUri(string userId)
        {
            return new Uri(_baseUri + ApiRoutes.Users.Get.Replace("{userId}", userId));
        }
    }
}
