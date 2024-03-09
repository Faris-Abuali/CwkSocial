using CwkSocial.Application.Models;
using CwkSocial.Domain.Aggregates.PostAggregate;
using ErrorOr;
using MediatR;

namespace CwkSocial.Application.Posts.AddPostComment;

public class AddPostCommentCommand
    : IRequest<ErrorOr<PostComment>>
{
    public Guid PostId { get; init; }
    public Guid UserProfileId { get; init; }
    public required string CommentText { get; init; }
}

