using CwkSocial.Application.Models;
using CwkSocial.DataAccess;
using CwkSocial.Domain.Aggregates.UserProfileAggregate;
using CwkSocial.Domain.Common.Errors;
using ErrorOr;
using MediatR;
using System.Net;

namespace CwkSocial.Application.UserProfiles.DeleteUserProfile;

internal class DeleteUserProfileCommandHandler : IRequestHandler<DeleteUserProfileCommand, ErrorOr<UserProfile>>
{
    private readonly DataContext _context;

    public DeleteUserProfileCommandHandler(DataContext context)
    {
        _context = context;
    }

    public async Task<ErrorOr<UserProfile>> Handle(DeleteUserProfileCommand request, CancellationToken cancellationToken)
    {
        var userProfile = await _context.UserProfiles.FindAsync(request.UserProfileId);

        if (userProfile is null)
            return Errors.User.UserProfileNotFound;

        _context.UserProfiles.Remove(userProfile);

        await _context.SaveChangesAsync(cancellationToken);

        return userProfile;
    }
}
