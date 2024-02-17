
using CwkSocial.Application.Models;
using CwkSocial.Domain.Aggregates.PostAggregate;
using MediatR;

namespace CwkSocial.Application.Posts.Commands;

public class CreatePostCommand : IRequest<OperationResult<Post>>
{
    public Guid UserProfileId { get; init; }
    public required string TextContent { get; init; }
}
