using AutoMapper;
using CwkSocial.Api.Contracts.Identity;
using CwkSocial.Api.Filters;
using CwkSocial.Application.Identity.Commands;
using CwkSocial.Application.Identity.ConfirmEmail;
using CwkSocial.Application.Identity.DeleteAccount;
using CwkSocial.Application.Identity.RegisterUser;
using CwkSocial.Infrastructure.Extensions;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace CwkSocial.Api.Controllers.V1;

[ApiVersion("1.0")]
[Route(ApiRoutes.BaseRoute)]
public class IdentityController : ApiController
{
    private readonly ISender _mediator;
    private readonly IMapper _mapper;
    private readonly IConfiguration _configuration;

    public IdentityController(ISender mediator, IMapper mapper, IConfiguration configuration)
    {
        _mediator = mediator;
        _mapper = mapper;
        _configuration = configuration;
    }

    [HttpPost]
    [Route(ApiRoutes.Identity.Registration)]
    [ValidateModel]
    public async Task<IActionResult> Register(RegisterUserRequest request)
    {
        // Construct the confirmation link path
        var confirmationPath = Url.Action(
            nameof(ConfirmEmail),
            "identity",
            new { email = request.UserName });
        // The token query string param will be added by the command handler

        var baseUrl = HttpContext.GetRequestBaseUrl();

        var command = _mapper.Map<RegisterUserCommand>(request);

        command.ConfirmationLink = $"{baseUrl}{confirmationPath}";

        var result = await _mediator.Send(command);

        return result.Match(
            token => Ok(new AuthenticationResponse { Token = token }),
            Problem);
    }

    [HttpPost]
    [Route(ApiRoutes.Identity.Login)]
    [ValidateModel]
    public async Task<IActionResult> Login(LoginRequest request)
    {
        var command = _mapper.Map<LoginCommand>(request);

        var result = await _mediator.Send(command);


        if (result.IsError) return Problem(result.Errors);

        var response = new AuthenticationResponse
        {
            Token = result.Value
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

        return result.Match(
            _ => NoContent(),
            Problem);
    }

    [HttpGet]
    [Route(ApiRoutes.Identity.ConfirmEmail)]
    public async Task<IActionResult> ConfirmEmail(
        [FromQuery] string email,
        [FromQuery] string token)
    {
        // Decode the email and token
        email = WebUtility.HtmlDecode(email);
        token = WebUtility.HtmlDecode(token);

        var command = new ConfirmEmailCommand
        {
            Email = email,
            Token = token,
        };

        var result = await _mediator.Send(command);

        return result.Match(
                _ => Ok(new { message = "Email Confirmed" }),
                Problem);
    }
}
