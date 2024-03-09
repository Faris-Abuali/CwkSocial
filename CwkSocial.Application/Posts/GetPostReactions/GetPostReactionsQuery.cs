
using ErrorOr;
using CwkSocial.Domain.Aggregates.PostAggregate;
using MediatR;

namespace CwkSocial.Application.Posts.GetPostReactions;

public class GetPostReactionsQuery
    : IRequest<ErrorOr<IEnumerable<PostReaction>>>
{
    public Guid PostId { get; init; }
}
