
using CwkSocial.Application.Models;
using CwkSocial.Domain.Aggregates.UserProfileAggregate;
using MediatR;

namespace CwkSocial.Application.UserProfiles.Commands;

public class DeleteUserProfileCommand : IRequest<OperationResult<UserProfile>>
{
    public Guid UserProfileId { get; set; }
}
