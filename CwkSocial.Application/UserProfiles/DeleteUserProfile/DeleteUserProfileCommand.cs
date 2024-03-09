using CwkSocial.Domain.Aggregates.UserProfileAggregate;
using ErrorOr;
using MediatR;

namespace CwkSocial.Application.UserProfiles.DeleteUserProfile;

public class DeleteUserProfileCommand : IRequest<ErrorOr<UserProfile>>
{
    public Guid UserProfileId { get; set; }
}
