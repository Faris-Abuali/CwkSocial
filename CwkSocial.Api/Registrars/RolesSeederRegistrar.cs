using CwkSocial.Domain.Enums;
using Microsoft.AspNetCore.Identity;

namespace CwkSocial.Api.Registrars;

public class RolesSeederRegistrar : IWebApplicationRegistrar
{
    public async void RegisterPipelineComponents(WebApplication app)
    {
        var scope = app.Services.CreateScope();
        var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();

        var roleNames = Enum.GetNames<UserRoles>();

        foreach (var roleName in roleNames)
        {
            if (!await roleManager.RoleExistsAsync(roleName))
            {
                await roleManager.CreateAsync(new IdentityRole(roleName));
            }
        }
    }
}
