using AutoMapper;
using CwkSocial.Api.Contracts.Identity;
using CwkSocial.Application.Identity.Commands;
using CwkSocial.Application.Identity.RegisterUser;

namespace CwkSocial.Api.MappingProfiles;

public class IdentityMappings : Profile
{
    public IdentityMappings()
    {
        CreateMap<RegisterUserRequest, RegisterUserCommand>();
        CreateMap<LoginRequest, LoginCommand>();
    }
}
