using CwkSocial.Application.Models;
using CwkSocial.Domain.Aggregates.UserProfileAggregate;
using MediatR;

namespace CwkSocial.Application.UserProfiles.Queries;

public class GetAllUserProfilesQuery : IRequest<OperationResult<IEnumerable<UserProfile>>>
{
    // TODO: Add properties for filtering, pagination, etc.
}
