
using CwkSocial.Application.Models;
using CwkSocial.Domain.Aggregates.PostAggregate;
using MediatR;

namespace CwkSocial.Application.Posts.Commands;

public class DeletePostCommand : IRequest<OperationResult<Post>>
{
    public required Guid PostId { get; init; }
    public required Guid UserProfileId { get; init; }
}
