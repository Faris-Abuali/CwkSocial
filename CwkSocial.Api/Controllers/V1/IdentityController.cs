using AutoMapper;
using CwkSocial.Api.Contracts.Identity;
using CwkSocial.Api.Filters;
using CwkSocial.Application.Identity.Commands;
using CwkSocial.Application.Identity.DeleteAccount;
using CwkSocial.Application.Identity.RegisterUser;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Vonage;
using Vonage.Request;

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
    //[ValidateModel]
    public async Task<IActionResult> Register(RegisterUserRequest request)
    {
        var command = _mapper.Map<RegisterUserCommand>(request);

        var result = await _mediator.Send(command);

        return result.Match(
            token => Ok(new AuthenticationResponse { Token = token }),
            Problem);
    }

    [HttpPost]
    [Route(ApiRoutes.Identity.Login)]
    //[ValidateModel]
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

    [HttpPost]
    [Route(ApiRoutes.Identity.Vonage)]
    public IActionResult Vonage()
    {
        var apiKey = _configuration["Vonage:ApiKey"];
        var apiSecret = _configuration["Vonage:ApiSecret"];

        var credentials = Credentials.FromApiKeyAndSecret(apiKey, apiSecret);

        var VonageClient = new VonageClient(credentials);

        var response = VonageClient.SmsClient.SendAnSms(new Vonage.Messaging.SendSmsRequest()
        {
            //To = "970562009983",
            To = "970592566124",
            From = "Vonage APIs",
            Text = "A text message sent using the Vonage SMS API"
        });

        return Ok(response);
    }
}
