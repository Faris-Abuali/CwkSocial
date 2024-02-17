
using CwkSocial.Application.Models;
using CwkSocial.Domain.Aggregates.PostAggregate;
using MediatR;

namespace CwkSocial.Application.Posts.Commands;

public class AddPostCommentCommand
    : IRequest<OperationResult<PostComment>>
{
    public Guid PostId { get; init; }
    public Guid UserProfileId { get; init; } //TODO: Take the UserProfileId from the token when JWT is implemented
    public required string CommentText { get; init; }
}

