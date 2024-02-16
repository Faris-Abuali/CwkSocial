using CwkSocial.Api.Contracts.Common;
using CwkSocial.Application.Models;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace CwkSocial.Api.Controllers.V1;

public class ApiController : ControllerBase
{
    protected IActionResult HandleErrorResponse(List<Error> errors)
    {
        Error? firstError;

        if (errors.Any(e => e.Code == HttpStatusCode.NotFound))
        {
            firstError = errors.FirstOrDefault(e => e.Code == HttpStatusCode.NotFound);

            return NotFound(new ErrorResponse
            {
                Phrase = "Not Found",
                Code = (int)HttpStatusCode.NotFound,
                Timestamp = DateTime.UtcNow,
                Errors = firstError is null ? [] : [firstError.Message]
            });
        }

        //if (errors.Any(e => e.Code == HttpStatusCode.InternalServerError))
        //{
        firstError = errors.FirstOrDefault(e => e.Code == HttpStatusCode.InternalServerError);

        return StatusCode(
            (int)HttpStatusCode.InternalServerError,
            new ErrorResponse
            {
                Phrase = "Internal Server Error",
                Code = (int)HttpStatusCode.InternalServerError,
                Timestamp = DateTime.UtcNow,
                Errors = firstError is null ? [] : [firstError.Message]
            });
        //}
    }
}
