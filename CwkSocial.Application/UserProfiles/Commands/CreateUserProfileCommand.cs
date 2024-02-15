using MediatR;
using CwkSocial.Domain.Aggregates.UserProfileAggregate;

namespace CwkSocial.Application.UserProfiles.Commands;

public class CreateUserProfileCommand : IRequest<UserProfile>
{
    public string FirstName { get; init; } = string.Empty;

    public string LastName { get; init; } = string.Empty;

    public string EmailAddress { get; init; } = string.Empty;

    public string Phone { get; init; } = string.Empty;

    public DateTime DateOfBirth { get; init; }

    public string CurrentCity { get; init; } = string.Empty;
}
