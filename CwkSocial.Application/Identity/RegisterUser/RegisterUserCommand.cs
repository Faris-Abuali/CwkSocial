using ErrorOr;
using MediatR;

namespace CwkSocial.Application.Identity.RegisterUser;

public class RegisterUserCommand : IRequest<ErrorOr<string>>
{
    public required string UserName { get; init; }

    public required string Password { get; init; }

    public required string FirstName { get; init; }

    public required string LastName { get; init; }

    public DateTime DateOfBirth { get; init; }

    public string? Phone { get; init; }

    public string? CurrentCity { get; init; }
}
