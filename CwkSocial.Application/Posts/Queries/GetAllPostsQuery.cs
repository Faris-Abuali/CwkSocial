
using CwkSocial.Application.Models;
using CwkSocial.Domain.Aggregates.PostAggregate;
using MediatR;

namespace CwkSocial.Application.Posts.Queries;

public class GetAllPostsQuery : IRequest<OperationResult<IEnumerable<Post>>>
{
}
