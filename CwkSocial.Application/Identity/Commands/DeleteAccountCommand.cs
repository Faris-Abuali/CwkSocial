

using CwkSocial.Application.Models;
using MediatR;

namespace CwkSocial.Application.Identity.Commands;

public class DeleteAccountCommand
    : IRequest<OperationResult<bool>>
{
    public required Guid IdentityUserId { get; init; }
    public required Guid UserProfileId { get; init; }
}
