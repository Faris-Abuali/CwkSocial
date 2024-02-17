
using CwkSocial.Application.Models;
using CwkSocial.Domain.Aggregates.PostAggregate;
using MediatR;

namespace CwkSocial.Application.Posts.Commands;

public class DeletePostCommand : IRequest<OperationResult<Post>>
{
    public Guid PostId { get; init; }
}
