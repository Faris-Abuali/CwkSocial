using ErrorOr;
using MediatR;

namespace CwkSocial.Application.Identity.Commands;

public class LoginCommand : IRequest<ErrorOr<string>>
{
    public required string UserName { get; init; }

    public required string Password { get; init; }
}
