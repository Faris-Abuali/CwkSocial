using AutoMapper;
using CwkSocial.Application.UserProfiles.Commands;
using CwkSocial.DataAccess;
using CwkSocial.Domain.Aggregates.UserProfileAggregate;
using MediatR;

namespace CwkSocial.Application.UserProfiles.CommandHandlers;

internal class UpdateUserProfileCommandHandler : IRequestHandler<UpdateUserProfileCommand, Unit>
{
    private readonly DataContext _context;

    public UpdateUserProfileCommandHandler(DataContext context)
    {
        _context = context;
    }

    public async Task<Unit> Handle(UpdateUserProfileCommand request, CancellationToken cancellationToken)
    {
        // Check if the user profile exists
        var userProfile = await _context.UserProfiles.FindAsync(request.UserProfileId);

        if (userProfile is null)
        {
            return new Unit();
        }

        // Create a new basic info object with the updated information
        var basicInfo = BasicInfo.Create(
                request.FirstName,
                request.LastName,
                request.EmailAddress,
                request.Phone,
                request.DateOfBirth,
                request.CurrentCity
        );

        // Update the user profile with the new information
        userProfile.UpdateBasicInfo(basicInfo);

        _context.UserProfiles.Update(userProfile);

        await _context.SaveChangesAsync(cancellationToken);

        return new Unit(); // Represents a void type, since System.Void is not a valid return type in 
    }
}
