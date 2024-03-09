using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace CwkSocial.Api.Controllers.V1;

public class ErrorsController : ControllerBase
{
    [Route("error")]
    public IActionResult Error()
    {
        // Get the exception
        var exceptionFeature = HttpContext.Features.Get<IExceptionHandlerFeature>();
        var exception = exceptionFeature?.Error;

        if (exception is null)
            return Problem();

        // Handle the exception as needed
        return Problem(
            statusCode: StatusCodes.Status500InternalServerError,
            title: exception.Message,
            detail: exception.InnerException?.Message);
    }
}
