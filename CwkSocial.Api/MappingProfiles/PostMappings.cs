using AutoMapper;
using CwkSocial.Api.Contracts.Post.Responses;
using CwkSocial.Api.Contracts.UserProfile.Responses;
using CwkSocial.Domain.Aggregates.PostAggregate;
using CwkSocial.Domain.Aggregates.UserProfileAggregate;

namespace CwkSocial.Api.MappingProfiles;

public class PostMappings : Profile
{
    public PostMappings()
    {
        CreateMap<Post, PostResponse>();
        CreateMap<PostComment, PostCommentResponse>();

        CreateMap<PostReaction, PostReactionResponse>()
            .ForMember(dest => dest.ReactionType, opt => opt.MapFrom(src => src.ReactionType.ToString()))
            .ForMember(dest => dest.Author, opt => opt.MapFrom(src => src.UserProfile));
    }
}
