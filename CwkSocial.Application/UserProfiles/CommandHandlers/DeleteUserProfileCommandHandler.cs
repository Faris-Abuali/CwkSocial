using CwkSocial.Application.Models;
using CwkSocial.Application.UserProfiles.Commands;
using CwkSocial.DataAccess;
using CwkSocial.Domain.Aggregates.UserProfileAggregate;
using MediatR;
using System.Net;

namespace CwkSocial.Application.UserProfiles.CommandHandlers;

internal class DeleteUserProfileCommandHandler : IRequestHandler<DeleteUserProfileCommand, OperationResult<UserProfile>>
{
    private readonly DataContext _context;

    public DeleteUserProfileCommandHandler(DataContext context)
    {
        _context = context;
    }

    public async Task<OperationResult<UserProfile>> Handle(DeleteUserProfileCommand request, CancellationToken cancellationToken)
    {
        var result = new OperationResult<UserProfile>();

        var userProfile = await _context.UserProfiles.FindAsync(request.UserProfileId);

        if (userProfile is null)
        {
            result.IsError = true;
            result.Errors.Add(
                new Error
                {
                    Code = HttpStatusCode.NotFound,
                    Message = $"No User profile found with ID: {request.UserProfileId}"
                }
            );

            return result;
        }

        _context.UserProfiles.Remove(userProfile);

        await _context.SaveChangesAsync(cancellationToken);

        result.Payload = userProfile;

        return result;
    }
}
