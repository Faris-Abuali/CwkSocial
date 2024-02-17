using MediatR;
using CwkSocial.Domain.Aggregates.UserProfileAggregate;
using CwkSocial.Application.Models;

namespace CwkSocial.Application.UserProfiles.Commands;

public class CreateUserProfileCommand : IRequest<OperationResult<UserProfile>>
{
    public string FirstName { get; init; } = string.Empty;

    public string LastName { get; init; } = string.Empty;

    public string EmailAddress { get; init; } = string.Empty;

    public string Phone { get; init; } = string.Empty;

    public DateTime DateOfBirth { get; init; }

    public string CurrentCity { get; init; } = string.Empty;
}
