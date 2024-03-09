using CwkSocial.Application.Models;
using CwkSocial.DataAccess;
using CwkSocial.Domain.Aggregates.UserProfileAggregate;
using CwkSocial.Domain.Common.Errors;
using CwkSocial.Domain.Exceptions;
using ErrorOr;
using MediatR;
using System.Net;

namespace CwkSocial.Application.UserProfiles.CreateUserProfile;

internal class CreateUserProfileCommandHandler : IRequestHandler<CreateUserProfileCommand, ErrorOr<UserProfile>>
{
    private readonly DataContext _context;

    public CreateUserProfileCommandHandler(DataContext context)
    {
        _context = context;
    }

    public async Task<ErrorOr<UserProfile>> Handle(
        CreateUserProfileCommand request,
        CancellationToken cancellationToken)
    {
        try
        {
            // Create a new basic info object
            var basicInfoResult = BasicInfo.Create(
                request.FirstName,
                request.LastName,
                request.EmailAddress,
                request.Phone,
                request.DateOfBirth,
                request.CurrentCity);

            if (basicInfoResult.IsError)
                return basicInfoResult.Errors;

            var basicInfo = basicInfoResult.Value;

            // Create a new user profile object with the basic info
            var userProfile = UserProfile.Create(Guid.NewGuid().ToString(), basicInfo);

            // Add the user profile to the database
            _context.UserProfiles.Add(userProfile);

            await _context.SaveChangesAsync(cancellationToken);

            return userProfile;
        }
        //catch (UserProfileNotValidException ex)
        //{
        //    ex.ValidationErrors
        //        .ForEach(msg => result.AddError(msg));
        //}
        catch (Exception ex)
        {
            return Errors.Unknown.Create(ex.Message);
        }
    }
}
