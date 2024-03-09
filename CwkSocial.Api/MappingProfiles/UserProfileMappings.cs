using AutoMapper;
using CwkSocial.Api.Contracts.Post.Responses;
using CwkSocial.Api.Contracts.UserProfile.Requests;
using CwkSocial.Api.Contracts.UserProfile.Responses;
using CwkSocial.Application.UserProfiles.CreateUserProfile;
using CwkSocial.Application.UserProfiles.UpdateUserProfile;
using CwkSocial.Domain.Aggregates.UserProfileAggregate;

namespace CwkSocial.Api.MappingProfiles;

public class UserProfileMappings : Profile
{
    public UserProfileMappings()
    {
        // Map from CreateUserProfileRequest (Contract) to CreateUserProfileCommand (Application)
        CreateMap<CreateUserProfileRequest, CreateUserProfileCommand>();

        // Map from UserProfile (Domain) to UserProfileResponse (Contract)
        CreateMap<UserProfile, UserProfileResponse>();

        // Map from BasicInfo (Domain) to BasicInformation (Contract)
        CreateMap<BasicInfo, BasicInformation>();

        CreateMap<CreateUserProfileRequest, UpdateUserProfileCommand>();

        CreateMap<UserProfile, ReactionAuthor>()
            .ForMember(dest => dest.FullName, opt => opt.MapFrom(src => $"{src.BasicInfo!.FirstName} {src.BasicInfo.LastName}"))
            .ForMember(dest => dest.City, opt => opt.MapFrom(src => src.BasicInfo!.CurrentCity))
            .ForMember(dest => dest.UserProfileId, opt => opt.MapFrom(src => src.UserProfileId));
    }
}
