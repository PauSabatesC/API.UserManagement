using AutoMapper;
using UserManagement.API.Controllers.v1.Contracts.Requests;
using UserManagement.API.Controllers.v1.Contracts.Requests.Queries;
using UserManagement.Domain.Entities;
using UserManagement.Services.DTOs.Requests;
using UserManagement.Services.DTOs.Responses;

namespace UserManagement.API.MappingProfiles
{
    public class ControllerReqToServiceReqProfile : Profile
    {
        public ControllerReqToServiceReqProfile()
        {
            CreateMap<CreateUserRequest, UserAuthenticationRequest>();
            CreateMap<UserRegistrationRequest, UserAuthenticationRequest>();
            CreateMap<UserLoginRequest, UserAuthenticationRequest>();
            CreateMap<UpdateUserRequest, UserAuthenticationRequest>();
            CreateMap<Controllers.v1.Contracts.Requests.RefreshTokenRequest, UserManagement.Services.DTOs.Requests.RefreshTokenRequest>();
            CreateMap<PaginationQuery, PaginationRequest>();
        }
    }
}
