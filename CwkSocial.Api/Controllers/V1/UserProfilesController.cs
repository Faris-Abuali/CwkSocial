using AutoMapper;
using CwkSocial.Api.Contracts.UserProfile.Requests;
using CwkSocial.Api.Contracts.UserProfile.Responses;
using CwkSocial.Api.Filters;
using CwkSocial.Application.UserProfiles.DeleteUserProfile;
using CwkSocial.Application.UserProfiles.GetAllUserProfiles;
using CwkSocial.Application.UserProfiles.GetUserProfileById;
using CwkSocial.Application.UserProfiles.UpdateUserProfile;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CwkSocial.Api.Controllers.V1;

[ApiVersion("1.0")]
[Route(ApiRoutes.BaseRoute)]
[Authorize()]
public class UserProfilesController : ApiController
{
    private readonly ISender _mediator;
    private readonly IMapper _mapper;

    public UserProfilesController(ISender mediator, IMapper mapper)
    {
        _mediator = mediator;
        _mapper = mapper;
    }

    [HttpGet]
    public async Task<IActionResult> GetAllProfiles()
    {
        var query = new GetAllUserProfilesQuery();

        var result = await _mediator.Send(query);

        return result.Match(
            profiles => Ok(_mapper.Map<List<UserProfileResponse>>(profiles)),
            Problem);
    }

    //[HttpPost]
    //public async Task<IActionResult> CreateUserProfile([FromBody] CreateUserProfileRequest profile)
    //{
    //    // Map CreateUserProfileRequest to CreateUserProfileCommand
    //    var command = _mapper.Map<CreateUserProfileCommand>(profile);

    //    // Send the command to the mediator
    //    var result = await _mediator.Send(command);

    //    if (result.IsError) return HandleErrorResponse(result.Errors);

    //    // Map the result from the Domain to the Contract
    //    var response = _mapper.Map<UserProfileResponse>(result.Payload);

    //    return CreatedAtAction(
    //        nameof(GetUserProfileById),
    //        new { id = response.UserProfileId }, response
    //    );
    //}

    [HttpGet]
    [Route(ApiRoutes.UserProfiles.IdRoute)]
    [ValidateModel]
    [ValidateGuid("id")]
    public async Task<IActionResult> GetUserProfileById(string id)
    {
        var query = new GetUserProfileByIdQuery
        {
            UserProfileId = Guid.Parse(id)
        };

        var result = await _mediator.Send(query);

        return result.Match(
            userProfile => Ok(_mapper.Map<UserProfileResponse>(userProfile)),
            Problem);
    }

    [HttpPatch]
    [Route(ApiRoutes.UserProfiles.IdRoute)]
    [ValidateModel]
    [ValidateGuid("id")]
    public async Task<IActionResult> UpdateUserProfile(string id, CreateUserProfileRequest updatedProfile)
    {
        var command = _mapper.Map<UpdateUserProfileCommand>(updatedProfile);

        command.UserProfileId = Guid.Parse(id);

        var result = await _mediator.Send(command);

        return result.Match(
            _ => NoContent(),
            Problem);
    }

    [HttpDelete]
    [Route(ApiRoutes.UserProfiles.IdRoute)]
    [ValidateGuid("id")]
    public async Task<IActionResult> DeleteUserProfile(string id)
    {
        var command = new DeleteUserProfileCommand
        {
            UserProfileId = Guid.Parse(id)
        };

        var result = await _mediator.Send(command);

        return result.Match(
            _ => NoContent(),
            Problem);
    }
}
