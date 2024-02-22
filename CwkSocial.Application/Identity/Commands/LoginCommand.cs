using CwkSocial.Application.Models;
using MediatR;

namespace CwkSocial.Application.Identity.Commands;

public class LoginCommand : IRequest<OperationResult<string>>
{
    public required string UserName { get; init; }

    public required string Password { get; init; }
}
