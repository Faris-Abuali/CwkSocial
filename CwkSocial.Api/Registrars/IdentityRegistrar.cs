using Microsoft.AspNetCore.Identity;

namespace CwkSocial.Api.Registrars;

public class IdentityRegistrar : IWebApplicationBuilderRegistrar
{
    public void RegisterServices(WebApplicationBuilder builder)
    {
        var jwtSettings = new JwtSettings();
        builder.Configuration.Bind(nameof(JwtSettings), jwtSettings);
        // this will bind the JwtSettings section from appsettings.json to the JwtSettings class

        // Now configure the JwtSettings class to be injected into the Dependency Injection container
        var jwtSection = builder.Configuration.GetSection(nameof(JwtSettings));
        builder.Services.Configure<JwtSettings>(jwtSection);

        // Add the authentication services
        builder.Services
            .AddAuthentication(options =>
            {
                // JWT bearer authentication will be used to authenticate users when they access protected resources.
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;

                // If authentication fails, the application will challenge the user with JWT bearer authentication.
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;

                // This ensures that JWT bearer authentication is the primary method for authenticating and authorizing users in the application.
                options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                options.SaveToken = true;
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(jwtSettings.SigningKey)),

                    ValidateIssuer = true,
                    ValidIssuer = jwtSettings.Issuer,

                    ValidateAudience = true,
                    ValidAudiences = jwtSettings.Audiences,

                    RequireExpirationTime = false,
                    ValidateLifetime = true
                };
                options.Audience = jwtSettings.Audiences[0];
                options.ClaimsIssuer = jwtSettings.Issuer;
            });
    }
}
