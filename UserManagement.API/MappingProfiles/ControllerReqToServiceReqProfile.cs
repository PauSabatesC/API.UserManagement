using AutoMapper;
using UserManagement.API.Controllers.v1.Contracts.Requests;
using UserManagement.Services.DTOs.Requests;

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
            CreateMap<Controllers.v1.Contracts.Requests.RefreshTokenRequest, Services.DTOs.Requests.RefreshTokenRequest>();

        }
    }
}
