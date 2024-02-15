using AutoMapper;
using CwkSocial.Application.UserProfiles.Commands;
using CwkSocial.Domain.Aggregates.UserProfileAggregate;

namespace CwkSocial.Application.MappingProfiles;

internal class UserProfileMap : Profile
{
    public UserProfileMap()
    {
        CreateMap<CreateUserProfileCommand, BasicInfo>();
    }
}
