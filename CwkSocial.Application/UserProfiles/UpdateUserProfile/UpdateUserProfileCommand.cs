using CwkSocial.Application.Models;
using CwkSocial.Domain.Aggregates.UserProfileAggregate;
using ErrorOr;
using MediatR;

namespace CwkSocial.Application.UserProfiles.UpdateUserProfile;

public class UpdateUserProfileCommand : IRequest<ErrorOr<UserProfile>>
{
    public Guid UserProfileId { get; set; }
    public string FirstName { get; init; } = string.Empty;
    public string LastName { get; init; } = string.Empty;
    public string EmailAddress { get; init; } = string.Empty;
    public string Phone { get; init; } = string.Empty;
    public DateTime DateOfBirth { get; init; }
    public string CurrentCity { get; init; } = string.Empty;
}
