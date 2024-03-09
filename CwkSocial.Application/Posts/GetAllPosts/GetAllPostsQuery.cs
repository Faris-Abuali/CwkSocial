using ErrorOr;
using CwkSocial.Domain.Aggregates.PostAggregate;
using MediatR;

namespace CwkSocial.Application.Posts.GetAllPosts;

public class GetAllPostsQuery : IRequest<ErrorOr<IEnumerable<Post>>>
{
}
