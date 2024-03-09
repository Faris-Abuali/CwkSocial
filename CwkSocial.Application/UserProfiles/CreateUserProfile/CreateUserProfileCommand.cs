using MediatR;
using CwkSocial.Domain.Aggregates.UserProfileAggregate;
using ErrorOr;

namespace CwkSocial.Application.UserProfiles.CreateUserProfile;

public class CreateUserProfileCommand : IRequest<ErrorOr<UserProfile>>
{
    public string FirstName { get; init; } = string.Empty;

    public string LastName { get; init; } = string.Empty;

    public string EmailAddress { get; init; } = string.Empty;

    public string Phone { get; init; } = string.Empty;

    public DateTime DateOfBirth { get; init; }

    public string CurrentCity { get; init; } = string.Empty;
}
