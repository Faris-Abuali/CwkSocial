using CwkSocial.Domain.Aggregates.UserProfileAggregate;
using ErrorOr;
using MediatR;

namespace CwkSocial.Application.UserProfiles.GetAllUserProfiles;

public class GetAllUserProfilesQuery : IRequest<ErrorOr<IEnumerable<UserProfile>>>
{
    // TODO: Add properties for filtering, pagination, etc.
}
