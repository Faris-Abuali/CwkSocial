using CwkSocial.Application.UserProfiles.Queries;
using CwkSocial.DataAccess;
using CwkSocial.Domain.Aggregates.UserProfileAggregate;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CwkSocial.Application.UserProfiles.QueryHandlers;

internal class GetAllUserProfilesHandler : IRequestHandler<GetAllUserProfiles, IEnumerable<UserProfile>>
{
    private readonly DataContext _context;

    public GetAllUserProfilesHandler(DataContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<UserProfile>> Handle(GetAllUserProfiles request, CancellationToken cancellationToken)
    {
        return await _context.UserProfiles.ToListAsync(cancellationToken);
    }
}
