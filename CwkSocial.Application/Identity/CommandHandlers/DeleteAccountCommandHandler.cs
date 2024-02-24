
using CwkSocial.Application.Identity.Commands;
using CwkSocial.Application.Models;
using CwkSocial.Application.UserProfiles;
using CwkSocial.DataAccess;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Net;

namespace CwkSocial.Application.Identity.CommandHandlers;

internal class DeleteAccountCommandHandler
    : IRequestHandler<DeleteAccountCommand, OperationResult<bool>>
{
    private readonly DataContext _context;

    public DeleteAccountCommandHandler(DataContext context)
    {
        _context = context;
    }

    public async Task<OperationResult<bool>> Handle(DeleteAccountCommand request, CancellationToken cancellationToken)
    {
        var result = new OperationResult<bool>();

        try
        {
            var identityUser = await _context.Users
                .FirstOrDefaultAsync(u => u.Id == request.IdentityUserId.ToString(), cancellationToken);

            if (identityUser is null)
            {
                result.AddError(IdentityErrorMessages.NonExistentIdentityUser, HttpStatusCode.NotFound);
                return result;
            }

            var userProfile = await _context.UserProfiles
                .FirstOrDefaultAsync(up => up.IdentityId == request.IdentityUserId.ToString(), cancellationToken);

            if (userProfile is null)
            {
                result.AddError(
                    string.Format(IdentityErrorMessages.IdentityUserWithNoUserProfile, request.IdentityUserId),
                    HttpStatusCode.NotFound);

                return result;
            }

            if (identityUser.Id != request.IdentityUserId.ToString())
            {
                result.AddError(
                    string.Format(IdentityErrorMessages.NotIdentityOwner, request.IdentityUserId),
                    HttpStatusCode.Forbidden);

                return result;
            }

            // Delete the user profile
            _context.UserProfiles.Remove(userProfile);

            // Delete the identity user
            _context.Users.Remove(identityUser);

            await _context.SaveChangesAsync(cancellationToken);

            result.Payload = true;
        }
        catch (Exception ex)
        {
            result.AddUnknownError(ex.Message);
        }

        return result;
    }
}
