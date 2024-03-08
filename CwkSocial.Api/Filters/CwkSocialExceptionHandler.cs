using CwkSocial.Api.Contracts.Common;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Net;

namespace CwkSocial.Api.Filters;

public class CwkSocialExceptionHandler : ExceptionFilterAttribute
{
    public override void OnException(ExceptionContext context)
    {
        base.OnException(context);

        var response = new ErrorResponse
        {
            Phrase = "Internal Server Error",
            Code = (int)HttpStatusCode.InternalServerError,
            Timestamp = DateTime.UtcNow,
            Errors = [context.Exception.Message]
        };

        context.Result = new JsonResult(response)
        {
            StatusCode = (int)HttpStatusCode.InternalServerError
        };

        context.ExceptionHandled = true;
    }
}
