
using CwkSocial.Application.Models;
using CwkSocial.Domain.Aggregates.PostAggregate;
using MediatR;

namespace CwkSocial.Application.Posts.Commands;

public class AddPostCommentCommand
    : IRequest<OperationResult<PostComment>>
{
    public Guid PostId { get; init; }
    public Guid UserProfileId { get; init; }
    public required string CommentText { get; init; }
}

