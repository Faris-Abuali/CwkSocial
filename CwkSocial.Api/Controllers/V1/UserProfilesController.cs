using AutoMapper;
using CwkSocial.Api.Contracts.Common;
using CwkSocial.Api.Contracts.UserProfile.Requests;
using CwkSocial.Api.Contracts.UserProfile.Responses;
using CwkSocial.Application.UserProfiles.Commands;
using CwkSocial.Application.UserProfiles.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace CwkSocial.Api.Controllers.V1;

[ApiVersion("1.0")]
[Route(ApiRoutes.BaseRoute)]
[ApiController]
public class UserProfilesController : ApiController
{
    private readonly IMediator _mediator;
    private readonly IMapper _mapper;

    public UserProfilesController(IMediator mediator, IMapper mapper)
    {
        _mediator = mediator;
        _mapper = mapper;
    }

    [HttpGet]
    public async Task<IActionResult> GetAllProfiles()
    {
        var query = new GetAllUserProfiles();

        var result = await _mediator.Send(query);

        var response = _mapper.Map<List<UserProfileResponse>>(result);

        return Ok(response);
    }

    [HttpPost]
    public async Task<IActionResult> CreateUserProfile([FromBody] CreateUserProfileRequest profile)
    {
        // Map CreateUserProfileRequest to CreateUserProfileCommand
        var command = _mapper.Map<CreateUserProfileCommand>(profile);

        // Send the command to the mediator
        var result = await _mediator.Send(command);

        // Map the result from the Domain to the Contract
        var response = _mapper.Map<UserProfileResponse>(result);

        return CreatedAtAction(
            nameof(GetUserProfileById),
            new { id = response.UserProfileId }, response
        );
    }

    [HttpGet]
    [Route(ApiRoutes.UserProfiles.IdRoute)]
    public async Task<IActionResult> GetUserProfileById(string id)
    {
        var query = new GetUserProfileById
        {
            UserProfileId = Guid.Parse(id)
        };

        var result = await _mediator.Send(query);

        if (result is null)
        {
            return NotFound($"No User with profile ID {id} found");
        }

        var response = _mapper.Map<UserProfileResponse>(result);

        return Ok(response);
    }

    [HttpPatch]
    [Route(ApiRoutes.UserProfiles.IdRoute)]
    public async Task<IActionResult> UpdateUserProfile(string id, CreateUserProfileRequest updatedProfile)
    {
        var command = _mapper.Map<UpdateUserProfileCommand>(updatedProfile);

        command.UserProfileId = Guid.Parse(id);

        var result = await _mediator.Send(command);

        return result.IsError ? HandleErrorResponse(result.Errors) : NoContent();
    }

    [HttpDelete]
    [Route(ApiRoutes.UserProfiles.IdRoute)]
    public async Task<IActionResult> DeleteUserProfile(string id)
    {
        var command = new DeleteUserProfileCommand
        {
            UserProfileId = Guid.Parse(id)
        };

        await _mediator.Send(command);

        return NoContent();
    }
}
