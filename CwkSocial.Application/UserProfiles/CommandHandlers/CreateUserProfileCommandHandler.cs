﻿using AutoMapper;
using CwkSocial.Application.Models;
using CwkSocial.Application.UserProfiles.Commands;
using CwkSocial.DataAccess;
using CwkSocial.Domain.Aggregates.UserProfileAggregate;
using CwkSocial.Domain.Exceptions;
using MediatR;
using System.Net;

namespace CwkSocial.Application.UserProfiles.CommandHandlers;

internal class CreateUserProfileCommandHandler : IRequestHandler<CreateUserProfileCommand, OperationResult<UserProfile>>
{
    private readonly DataContext _context;

    public CreateUserProfileCommandHandler(DataContext context)
    {
        _context = context;
    }

    public async Task<OperationResult<UserProfile>> Handle(CreateUserProfileCommand request, CancellationToken cancellationToken)
    {
        var result = new OperationResult<UserProfile>();

        try
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

            result.Payload = userProfile;

            return result;
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

            result.Errors.Add(new Error
            {
                Code = HttpStatusCode.InternalServerError,
                Message = ex.Message,
            });

            return result;
        }
    }
}
