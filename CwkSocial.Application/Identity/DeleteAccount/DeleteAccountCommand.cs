using ErrorOr;
using MediatR;

namespace CwkSocial.Application.Identity.DeleteAccount;

public class DeleteAccountCommand
    : IRequest<ErrorOr<bool>>
{
    public required Guid IdentityUserId { get; init; }
    public required Guid UserProfileId { get; init; }
}
