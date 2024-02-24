using AutoMapper;
using CwkSocial.Api.Contracts.Identity;
using CwkSocial.Api.Filters;
using CwkSocial.Application.Identity.Commands;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CwkSocial.Api.Controllers.V1;

[ApiVersion("1.0")]
[Route(ApiRoutes.BaseRoute)]
[ApiController]
public class IdentityController : ApiController
{
    private readonly ISender _mediator;
    private readonly IMapper _mapper;

    public IdentityController(ISender mediator, IMapper mapper)
    {
        _mediator = mediator;
        _mapper = mapper;
    }

    [HttpPost]
    [Route(ApiRoutes.Identity.Registration)]
    [ValidateModel]
    public async Task<IActionResult> Register(RegisterUserRequest request)
    {
        var command = _mapper.Map<RegisterUserCommand>(request);

        var result = await _mediator.Send(command);

        if (result.IsError) return HandleErrorResponse(result.Errors);

        var response = new AuthenticationResponse
        {
            Token = result.Payload!
        };

        return Ok(response);
    }

    [HttpPost]
    [Route(ApiRoutes.Identity.Login)]
    [ValidateModel]
    public async Task<IActionResult> Login(LoginRequest request)
    {
        var command = _mapper.Map<LoginCommand>(request);

        var result = await _mediator.Send(command);

        if (result.IsError) return HandleErrorResponse(result.Errors);

        var response = new AuthenticationResponse
        {
            Token = result.Payload!
        };

        return Ok(response);
    }

    [HttpDelete]
    [Route(ApiRoutes.Identity.IdentityById)]
    [ValidateGuid("identityUserId")]
    [Authorize()]
    public async Task<IActionResult> DeleteAccount(string identityUserId, CancellationToken cancellationToken)
    {
        var userProfileId = HttpContext.GetUserProfileIdClaimValue();

        var command = new DeleteAccountCommand
        {
            IdentityUserId = Guid.Parse(identityUserId),
            UserProfileId = userProfileId
        };

        var result = await _mediator.Send(command, cancellationToken);

        if (result.IsError) return HandleErrorResponse(result.Errors);

        return NoContent();
    }
}
