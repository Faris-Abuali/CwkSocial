using AutoMapper;
using CwkSocial.Application.Enums;
using CwkSocial.Application.Models;
using CwkSocial.DataAccess;
using CwkSocial.Domain.Aggregates.UserProfileAggregate;
using CwkSocial.Domain.Common.Errors;
using ErrorOr;
using MediatR;

namespace CwkSocial.Application.UserProfiles.UpdateUserProfile;

internal class UpdateUserProfileCommandHandler
    : IRequestHandler<UpdateUserProfileCommand, ErrorOr<UserProfile>>
{
    private readonly DataContext _context;

    public UpdateUserProfileCommandHandler(DataContext context)
    {
        _context = context;
    }

    public async Task<ErrorOr<UserProfile>> Handle(UpdateUserProfileCommand request, CancellationToken cancellationToken)
    {
        try
        {
            // Check if the user profile exists
            var userProfile = await _context.UserProfiles.FindAsync(request.UserProfileId);

            if (userProfile is null)
                return Errors.User.UserProfileNotFound;


            // Create a new basic info object with the updated information
            var basicInfoResult = BasicInfo.Create(
                    request.FirstName,
                    request.LastName,
                    request.EmailAddress,
                    request.Phone,
                    request.DateOfBirth,
                    request.CurrentCity
            );

            if (basicInfoResult.IsError)
                return basicInfoResult.Errors;

            var basicInfo = basicInfoResult.Value;

            // Update the user profile with the new information
            userProfile.UpdateBasicInfo(basicInfo);

            _context.UserProfiles.Update(userProfile);

            await _context.SaveChangesAsync(cancellationToken);

            return userProfile;
        }
        //catch (UserProfileNotValidException ex)
        //{
        //    ex.ValidationErrors
        //        .ForEach(msg => result.AddError(msg));

        //    return result;
        //}
        catch (Exception ex)
        {
            return Errors.Unknown.Create(ex.Message);
        }
    }
}
