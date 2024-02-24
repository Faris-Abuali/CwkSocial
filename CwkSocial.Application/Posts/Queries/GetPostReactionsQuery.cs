
using CwkSocial.Application.Models;
using CwkSocial.Domain.Aggregates.PostAggregate;
using MediatR;

namespace CwkSocial.Application.Posts.Queries;

public class GetPostReactionsQuery
    : IRequest<OperationResult<IEnumerable<PostReaction>>>
{
    public Guid PostId { get; init; }
}
