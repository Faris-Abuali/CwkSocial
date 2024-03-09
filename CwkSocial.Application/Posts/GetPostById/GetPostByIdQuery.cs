
using ErrorOr;
using CwkSocial.Domain.Aggregates.PostAggregate;
using MediatR;

namespace CwkSocial.Application.Posts.GetPostById;

public class GetPostByIdQuery : IRequest<ErrorOr<Post>>
{
    public Guid PostId { get; init; }
}
