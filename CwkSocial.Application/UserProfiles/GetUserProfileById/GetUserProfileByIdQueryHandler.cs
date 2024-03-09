using CwkSocial.DataAccess;
using CwkSocial.Domain.Aggregates.UserProfileAggregate;
using CwkSocial.Domain.Common.Errors;
using ErrorOr;
using MediatR;

namespace CwkSocial.Application.UserProfiles.GetUserProfileById;

internal class GetUserProfileByIdQueryHandler : IRequestHandler<GetUserProfileByIdQuery, ErrorOr<UserProfile?>>
{
    private readonly DataContext _context;

    public GetUserProfileByIdQueryHandler(DataContext context)
    {
        _context = context;
    }

    public async Task<ErrorOr<UserProfile?>> Handle(GetUserProfileByIdQuery request, CancellationToken cancellationToken)
    {
        var userProfile = await _context.UserProfiles.FindAsync(request.UserProfileId);

        if (userProfile is null)
            return Errors.User.UserProfileNotFound;

        return userProfile;
    }
}
