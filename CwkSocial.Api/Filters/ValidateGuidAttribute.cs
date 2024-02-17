using CwkSocial.Api.Contracts.Common;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Net;

namespace CwkSocial.Api.Filters;

public class ValidateGuidAttribute : ActionFilterAttribute
{
    private readonly string _key;

    public ValidateGuidAttribute(string key)
    {
        _key = key;
    }

    public override void OnActionExecuting(ActionExecutingContext context)
    {
        if (!context.ActionArguments.TryGetValue(_key, out var value)) return;

        // Now the variable called 'value' contains the value of the parameter
        if (Guid.TryParse(value?.ToString(), out var guid)) return;

        var response = new ErrorResponse
        {
            Code = (int)HttpStatusCode.BadRequest,
            Phrase = "Bad Request",
            Timestamp = DateTime.UtcNow,
            Errors = [$"The parameter {_key} is not a valid GUID format: {value}"]
        };

        context.Result = new JsonResult(response);
    }
}
