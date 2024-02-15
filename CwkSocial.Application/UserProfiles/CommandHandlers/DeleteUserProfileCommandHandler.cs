using CwkSocial.Application.UserProfiles.Commands;
using CwkSocial.DataAccess;
using MediatR;

namespace CwkSocial.Application.UserProfiles.CommandHandlers;

internal class DeleteUserProfileCommandHandler : IRequestHandler<DeleteUserProfileCommand, Unit>
{
    private readonly DataContext _context;

    public DeleteUserProfileCommandHandler(DataContext context)
    {
        _context = context;
    }

    public async Task<Unit> Handle(DeleteUserProfileCommand request, CancellationToken cancellationToken)
    {
        var userProfile = await _context.UserProfiles.FindAsync(request.UserProfileId);
        
        if (userProfile is null)
        {
            return new Unit();
        }

        _context.UserProfiles.Remove(userProfile);

        await _context.SaveChangesAsync(cancellationToken);

        return new Unit();
    }
}
