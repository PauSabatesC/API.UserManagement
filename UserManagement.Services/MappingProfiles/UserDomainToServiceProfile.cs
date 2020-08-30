using AutoMapper;
using System;
using System.Collections.Generic;
using System.Text;
using UserManagement.Domain.Entities;
using UserManagement.Services.DTOs.Responses;

namespace UserManagement.Services.MappingProfiles
{
    public class UserDomainToServiceProfile : Profile
    {
        public UserDomainToServiceProfile()
        {
            CreateMap<User, UserResponse>();
        }
    }
}
