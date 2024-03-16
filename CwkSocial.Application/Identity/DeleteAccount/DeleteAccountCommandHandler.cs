using CwkSocial.DataAccess;
using CwkSocial.Domain.Common.Errors;
using ErrorOr;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CwkSocial.Application.Identity.DeleteAccount;

internal class DeleteAccountCommandHandler
    : IRequestHandler<DeleteAccountCommand, ErrorOr<bool>>
{
    private readonly DataContext _context;

    public DeleteAccountCommandHandler(DataContext context)
    {
        _context = context;
    }

    public async Task<ErrorOr<bool>> Handle(DeleteAccountCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var identityUser = await _context.Users
            .FirstOrDefaultAsync(u => u.Id == request.IdentityUserId.ToString(), cancellationToken);

            if (identityUser is null)
                return Errors.Identity.UserNotFound;

            var userProfile = await _context.UserProfiles
                .FirstOrDefaultAsync(up => up.IdentityId == request.IdentityUserId.ToString(), cancellationToken);

            if (userProfile is null)
                return Errors.User.UserProfileNotFound;

            if (identityUser.Id != request.IdentityUserId.ToString())
                return Errors.Identity.NotIdentityOwner(request.IdentityUserId.ToString());

            // Delete the user profile
            _context.UserProfiles.Remove(userProfile);

            // Delete the identity user
            _context.Users.Remove(identityUser);

            // Delete the roles of the user
            var roles = await _context.UserRoles
                .Where(ur => ur.UserId == request.IdentityUserId.ToString())
                .ToListAsync(cancellationToken);

            _context.UserRoles.RemoveRange(roles);

            await _context.SaveChangesAsync(cancellationToken);

            return true; // successfully deleted
        }
        catch (Exception ex)
        {
            return Errors.Unknown.Create(ex.Message);
        }
    }
}
