using CwkSocial.Domain.Aggregates.PostAggregate;
using ErrorOr;
using MediatR;

namespace CwkSocial.Application.Posts.AddPostReaction;

public class AddPostReactionCommand
    : IRequest<ErrorOr<PostReaction>>
{
    public required Guid PostId { get; init; }
    public required Guid UserProfileId { get; init; }
    public required ReactionType ReactionType { get; init; }
}

