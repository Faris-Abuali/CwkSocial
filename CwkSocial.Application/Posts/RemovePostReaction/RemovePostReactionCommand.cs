using CwkSocial.Domain.Aggregates.PostAggregate;
using MediatR;
using ErrorOr;

namespace CwkSocial.Application.Posts.RemovePostReaction;

public class RemovePostReactionCommand
    : IRequest<ErrorOr<PostReaction>>
{
    public required Guid PostId { get; init; }
    public required Guid ReactionId { get; init; }
    public required Guid UserProfileId { get; init; }
}
