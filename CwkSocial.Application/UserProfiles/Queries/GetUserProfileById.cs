using CwkSocial.Domain.Aggregates.UserProfileAggregate;
using MediatR;

namespace CwkSocial.Application.UserProfiles.Queries;

public class GetUserProfileById : IRequest<UserProfile?>
{
    public Guid UserProfileId { get; init; }
}
