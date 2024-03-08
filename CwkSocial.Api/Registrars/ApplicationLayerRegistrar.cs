
using CwkSocial.Application.Common.Behaviors;
using CwkSocial.Application.Identity.Commands;
using CwkSocial.Application.Identity.Validators;
using CwkSocial.Application.Services;
using CwkSocial.Application.UserProfiles.Queries;
using ErrorOr;
using FluentValidation;
using MediatR;
using System.Reflection;

namespace CwkSocial.Api.Registrars;

public class ApplicationLayerRegistrar : IWebApplicationBuilderRegistrar
{
    public void RegisterServices(WebApplicationBuilder builder)
    {
        builder.Services.AddScoped<IdentityService>();

        /**
          * 1. Scan the assembly where Porgram class exists (Api layer)
          * 2. Scan the assembly for any class from the (Application layer) (e.g. GetAllUserProfiles)
          */
        builder.Services.AddAutoMapper(typeof(Program), typeof(GetAllUserProfilesQuery));


        //builder.Services.AddScoped<
        //    IPipelineBehavior<RegisterUserCommand, ErrorOr<string>>,
        //    ValidateRegisterCommandBehavior>();


        //// This way we add validation for one use-case only 👇😓
        //builder.Services.AddScoped<IValidator<RegisterUserCommand>, RegisterUserCommandValidator>();
    }
}
