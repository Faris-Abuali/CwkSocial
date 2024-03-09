using CwkSocial.Domain.Aggregates.PostAggregate;
using ErrorOr;
using MediatR;

namespace CwkSocial.Application.Posts.CreatePost;

public class CreatePostCommand : IRequest<ErrorOr<Post>>
{
    public required Guid UserProfileId { get; init; }
    public required string TextContent { get; init; }
}
