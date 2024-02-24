
using CwkSocial.Application.Models;
using CwkSocial.Domain.Aggregates.PostAggregate;
using MediatR;

namespace CwkSocial.Application.Posts.Commands;

public class AddPostReactionCommand
    : IRequest<OperationResult<PostReaction>>
{
    public required Guid PostId { get; init; }
    public required Guid UserProfileId { get; init; }
    public required ReactionType ReactionType { get; init; }
}

