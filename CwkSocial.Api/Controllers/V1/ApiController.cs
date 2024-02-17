using CwkSocial.Api.Contracts.Common;
using CwkSocial.Application.Models;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace CwkSocial.Api.Controllers.V1;

public class ApiController : ControllerBase
{
    protected IActionResult HandleErrorResponse(List<Error> errors)
    {
        var firstError = errors.First();

        if (firstError is null)
        {
            return StatusCode(
            (int)HttpStatusCode.InternalServerError,
            new ErrorResponse
            {
                Phrase = "Internal Server Error",
                Code = (int)HttpStatusCode.InternalServerError,
                Timestamp = DateTime.UtcNow,
                Errors = []
            });

        }

        var response = new ErrorResponse
        {
            Phrase = "Bad Request",
            Code = (int)firstError.Code,
            Timestamp = DateTime.UtcNow,
            Errors = errors.ConvertAll(err => err.Message)
        };

        return new JsonResult(response)
        {
            StatusCode = (int)firstError.Code
        };
    }
}
