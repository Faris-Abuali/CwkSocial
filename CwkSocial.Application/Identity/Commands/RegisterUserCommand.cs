using CwkSocial.Application.Models;
using MediatR;

namespace CwkSocial.Application.Identity.Commands;

public class RegisterUserCommand : IRequest<OperationResult<string>>
{
    public required string UserName { get; init; }

    public required string Password { get; init; }

    public required string FirstName { get; init; }

    public required string LastName { get; init; }

    public DateTime DateOfBirth { get; init; }

    public string? Phone { get; init; }

    public string? CurrentCity { get; init; }
}
