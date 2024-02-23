using CwkSocial.Application.Models;
using CwkSocial.Application.UserProfiles.Queries;
using CwkSocial.DataAccess;
using CwkSocial.Domain.Aggregates.UserProfileAggregate;
using MediatR;
using System.Net;

namespace CwkSocial.Application.UserProfiles.QueryHandlers;

internal class GetUserProfileByIdQueryHandler : IRequestHandler<GetUserProfileByIdQuery, OperationResult<UserProfile?>>
{
    private readonly DataContext _context;

    public GetUserProfileByIdQueryHandler(DataContext context)
    {
        _context = context;
    }

    public async Task<OperationResult<UserProfile?>> Handle(GetUserProfileByIdQuery request, CancellationToken cancellationToken)
    {
        var result = new OperationResult<UserProfile?>();

        var userProfile = await _context.UserProfiles.FindAsync(request.UserProfileId);

        if (userProfile is null)
        {
            result.AddError(
                    string.Format(UserProfilesErrorMessages.UserProfileNotFound, request.UserProfileId),
                    HttpStatusCode.NotFound);

            return result;
        }

        result.Payload = userProfile;

        return result;
    }
}
