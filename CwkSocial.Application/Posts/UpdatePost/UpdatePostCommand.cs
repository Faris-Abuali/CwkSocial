
using CwkSocial.Domain.Aggregates.PostAggregate;
using MediatR;
using ErrorOr;

namespace CwkSocial.Application.Posts.UpdatePost;

public class UpdatePostCommand : IRequest<ErrorOr<Post>>
{
    public required Guid PostId { get; init; }
    public required string TextContent { get; init; }
    public required Guid UserProfileId { get; init; }
}
