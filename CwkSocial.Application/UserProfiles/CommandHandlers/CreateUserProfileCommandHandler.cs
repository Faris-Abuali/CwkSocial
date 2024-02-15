using AutoMapper;
using CwkSocial.Application.UserProfiles.Commands;
using CwkSocial.DataAccess;
using CwkSocial.Domain.Aggregates.UserProfileAggregate;
using MediatR;

namespace CwkSocial.Application.UserProfiles.CommandHandlers;

internal class CreateUserProfileCommandHandler : IRequestHandler<CreateUserProfileCommand, UserProfile>
{
    private readonly DataContext _context;

    public CreateUserProfileCommandHandler(DataContext context)
    {
        _context = context;
    }

    public async Task<UserProfile> Handle(CreateUserProfileCommand request, CancellationToken cancellationToken)
    {
        // Create a new basic info object
        var basicInfo = BasicInfo.Create(
            request.FirstName,
            request.LastName,
            request.EmailAddress,
            request.Phone,
            request.DateOfBirth,
            request.CurrentCity);

        // Create a new user profile object with the basic info
        var userProfile = UserProfile.Create(Guid.NewGuid().ToString(), basicInfo);

        // Add the user profile to the database
        _context.UserProfiles.Add(userProfile);

        await _context.SaveChangesAsync(cancellationToken);

        return userProfile;
    }
}
