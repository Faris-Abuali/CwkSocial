using CwkSocial.Domain.Aggregates.UserProfileAggregate;
using ErrorOr;
using MediatR;

namespace CwkSocial.Application.UserProfiles.GetUserProfileById;

public class GetUserProfileByIdQuery : IRequest<ErrorOr<UserProfile?>>
{
    public Guid UserProfileId { get; init; }
}
