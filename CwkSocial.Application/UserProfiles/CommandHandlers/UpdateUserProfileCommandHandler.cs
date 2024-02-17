using AutoMapper;
using CwkSocial.Application.Enums;
using CwkSocial.Application.Models;
using CwkSocial.Application.UserProfiles.Commands;
using CwkSocial.DataAccess;
using CwkSocial.Domain.Aggregates.UserProfileAggregate;
using CwkSocial.Domain.Exceptions;
using MediatR;
using System.Net;

namespace CwkSocial.Application.UserProfiles.CommandHandlers;

internal class UpdateUserProfileCommandHandler : IRequestHandler<UpdateUserProfileCommand, OperationResult<UserProfile>>
{
    private readonly DataContext _context;

    public UpdateUserProfileCommandHandler(DataContext context)
    {
        _context = context;
    }

    public async Task<OperationResult<UserProfile>> Handle(UpdateUserProfileCommand request, CancellationToken cancellationToken)
    {
        var result = new OperationResult<UserProfile>();

        try
        {
            // Check if the user profile exists
            var userProfile = await _context.UserProfiles.FindAsync(request.UserProfileId);

            if (userProfile is null)
            {
                result.IsError = true;
                var error = new Error { Code = HttpStatusCode.NotFound, Message = $"No User profile found with ID: {request.UserProfileId}" };
                result.Errors.Add(error);
                return result;
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

            result.Payload = userProfile;
        }
        catch (UserProfileNotValidException ex)
        {
            result.IsError = true;

            result.Errors = ex.ValidationErrors
                .ConvertAll(err => new Error
                {
                    Code = HttpStatusCode.BadRequest,
                    Message = err,
                });

            return result;
        }
        catch (Exception ex)
        {
            result.IsError = true;
            result.Errors.Add(new Error { Message = ex.Message, Code = HttpStatusCode.InternalServerError });
        }

        return result;
    }
}
