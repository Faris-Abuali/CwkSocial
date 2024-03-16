
using ErrorOr;
using MediatR;

namespace CwkSocial.Application.Identity.ConfirmEmail;

public class ConfirmEmailCommand : IRequest<ErrorOr<Unit>>
{
    public required string Token { get; init; }
    public required string Email { get; init; }
}
