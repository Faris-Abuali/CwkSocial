using CwkSocial.Domain.Aggregates.PostAggregate;
using ErrorOr;
using MediatR;

namespace CwkSocial.Application.Posts.DeletePost;

public class DeletePostCommand : IRequest<ErrorOr<Post>>
{
    public required Guid PostId { get; init; }
    public required Guid UserProfileId { get; init; }
}
