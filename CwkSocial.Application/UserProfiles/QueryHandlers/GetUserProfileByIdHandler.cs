using CwkSocial.Application.Models;
using CwkSocial.Application.UserProfiles.Queries;
using CwkSocial.DataAccess;
using CwkSocial.Domain.Aggregates.UserProfileAggregate;
using MediatR;
using System.Net;

namespace CwkSocial.Application.UserProfiles.QueryHandlers;

internal class GetUserProfileByIdHandler : IRequestHandler<GetUserProfileById, OperationResult<UserProfile?>>
{
    private readonly DataContext _context;

    public GetUserProfileByIdHandler(DataContext context)
    {
        _context = context;
    }

    public async Task<OperationResult<UserProfile?>> Handle(GetUserProfileById request, CancellationToken cancellationToken)
    {
        var result = new OperationResult<UserProfile?>();

        var userProfile = await _context.UserProfiles.FindAsync(request.UserProfileId);

        if (userProfile is null)
        {
            result.IsError = true;
            result.Errors.Add(new Error
            {
                Code = HttpStatusCode.NotFound,
                Message = $"No User profile found with ID: {request.UserProfileId}"
            });
            return result;
        }

        result.Payload = userProfile;

        return result;
    }
}
