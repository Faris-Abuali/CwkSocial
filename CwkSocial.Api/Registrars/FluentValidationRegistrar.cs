
using CwkSocial.Application.Common.Behaviors;
using CwkSocial.Application.UserProfiles.Queries;
using FluentValidation;
using MediatR;
using System.Reflection;

namespace CwkSocial.Api.Registrars;

public class FluentValidationRegistrar : IWebApplicationBuilderRegistrar
{
    public void RegisterServices(WebApplicationBuilder builder)
    {
        builder.Services.AddScoped(
            typeof(IPipelineBehavior<,>),
            typeof(FluentValidationBehavior<,>));

        // Include validators from the current assembly (API layer)
        builder.Services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());

        // Include validators from the application layer assembly
        builder.Services.AddValidatorsFromAssembly(typeof(GetAllUserProfilesQuery).Assembly);
    }
}
