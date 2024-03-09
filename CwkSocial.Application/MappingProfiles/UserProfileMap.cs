using AutoMapper;
using CwkSocial.Application.UserProfiles.CreateUserProfile;
using CwkSocial.Domain.Aggregates.UserProfileAggregate;

namespace CwkSocial.Application.MappingProfiles;

internal class UserProfileMap : Profile
{
    public UserProfileMap()
    {
        CreateMap<CreateUserProfileCommand, BasicInfo>();
    }
}
