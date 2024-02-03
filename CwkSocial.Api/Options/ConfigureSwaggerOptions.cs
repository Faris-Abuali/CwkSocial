using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.Extensions.Options;
using Swashbuckle.AspNetCore.SwaggerGen;
using Microsoft.OpenApi.Models;

namespace CwkSocial.Api.Options;

public class ConfigureSwaggerOptions : IConfigureOptions<SwaggerGenOptions>
{
    private readonly IApiVersionDescriptionProvider _provider;

    public ConfigureSwaggerOptions(IApiVersionDescriptionProvider provider)
    {
        _provider = provider;
    }

    public void Configure(SwaggerGenOptions options)
    {
        // Generate a swagger document for each discovered API version
        foreach (var description in _provider.ApiVersionDescriptions)
        {
            options.SwaggerDoc(description.GroupName, CreateVersionInfo(description));
        }
    }

    private OpenApiInfo CreateVersionInfo(ApiVersionDescription description)
    {
        var info = new OpenApiInfo
        {
            Title = "CwkSocial",
            Version = description.ApiVersion.ToString()
        };

        if (description.IsDeprecated)
        {
            info.Description = "This API version has been deprecated";
        }

        return info;
    }
}
