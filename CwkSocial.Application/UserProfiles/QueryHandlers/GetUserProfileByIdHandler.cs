using CwkSocial.Application.UserProfiles.Queries;
using CwkSocial.DataAccess;
using CwkSocial.Domain.Aggregates.UserProfileAggregate;
using MediatR;

namespace CwkSocial.Application.UserProfiles.QueryHandlers;

internal class GetUserProfileByIdHandler : IRequestHandler<GetUserProfileById, UserProfile?>
{
    private readonly DataContext _context;

    public GetUserProfileByIdHandler(DataContext context)
    {
        _context = context;
    }

    public async Task<UserProfile?> Handle(GetUserProfileById request, CancellationToken cancellationToken)
    {
       return await _context.UserProfiles.FindAsync(request.UserProfileId, cancellationToken);
    }
}
