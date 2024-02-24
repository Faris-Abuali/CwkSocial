using CwkSocial.Application.Models;
using CwkSocial.Domain.Aggregates.PostAggregate;
using MediatR;

namespace CwkSocial.Application.Posts.Commands;

public class RemovePostReactionCommand
    : IRequest<OperationResult<PostReaction>>
{
    public required Guid PostId { get; init; }
    public required Guid ReactionId { get; init; }
    public required Guid UserProfileId { get; init; }
}
