using CwkSocial.Domain.Aggregates.PostAggregate;
using ErrorOr;
using MediatR;

namespace CwkSocial.Application.Posts.GetPostComments;

public class GetPostCommentsQuery : IRequest<ErrorOr<IEnumerable<PostComment>>>
{
    public Guid PostId { get; set; }
}
