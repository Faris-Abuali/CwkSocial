using CwkSocial.DataAccess;
using CwkSocial.Domain.Aggregates.UserProfileAggregate;
using ErrorOr;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CwkSocial.Application.UserProfiles.GetAllUserProfiles;

internal class GetAllUserProfilesQueryHandler : IRequestHandler<GetAllUserProfilesQuery, ErrorOr<IEnumerable<UserProfile>>>
{
    private readonly DataContext _context;

    public GetAllUserProfilesQueryHandler(DataContext context)
    {
        _context = context;
    }

    public async Task<ErrorOr<IEnumerable<UserProfile>>> Handle(GetAllUserProfilesQuery request, CancellationToken cancellationToken)
    {
        var userProfiles = await _context.UserProfiles.ToListAsync(cancellationToken);

        return userProfiles;
    }
}
