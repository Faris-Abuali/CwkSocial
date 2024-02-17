using CwkSocial.Application.UserProfiles.Queries;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace CwkSocial.Api.Registrars;

public class MediatRRegistrar : IWebApplicationBuilderRegistrar
{
    public void RegisterServices(WebApplicationBuilder builder)
    {
        // Add AutoMapper
        /**
         * 1. Scan the assembly where Porgram class exists (Api layer)
         * 2. Scan the assembly for any class from the (Application layer) (e.g. GetAllUserProfiles)
        */
        builder.Services.AddAutoMapper(typeof(Program), typeof(GetAllUserProfilesQuery));

        // Add MediatR
        builder.Services.AddMediatR(cfg =>
        {
            // Alternatively, register handlers from a specific assembly:
            cfg.RegisterServicesFromAssembly(typeof(GetAllUserProfilesQuery).Assembly);
        });
    }
}
