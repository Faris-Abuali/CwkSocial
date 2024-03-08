using CwkSocial.Api.Common.Http;
using CwkSocial.Api.Contracts.Common;
using ErrorOr;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.Net;
using AppError = CwkSocial.Application.Models.Error;

namespace CwkSocial.Api.Controllers.V1;

[ApiController]
public class ApiController : ControllerBase
{
    protected IActionResult HandleErrorResponse(List<AppError> errors)
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

    /// <summary>
    /// Overrides the ControllerBase.Problem to customize our error structure
    /// </summary>
    /// <param name="errors">A list of ErrorOr.Error</param>
    /// <returns></returns>
    protected IActionResult Problem(List<Error> errors)
    {
        if (errors.Count is 0)
        {
            return Problem(); // No errors, return a generic problem
        }

        /**
         * If all errors are validation errors, call the ValidationProblem 
         * method which will return a 400 status code, plus a list of all
         * validation errors:
         */
        if (errors.All(error => error.Type == ErrorType.Validation))
        {
            return ValidationProblem(errors);
        }

        // `HttpContext.Items` Gets or sets a key/value collection that can be used to share data within the scope of this request.
        HttpContext.Items.Add(HttpContextItemKeys.Errors, errors);
        // 👆 We will only return the first error to the client, however we store all errors in the HttpContext.Items dictionary for logging purposes.

        var firstError = errors.First();

        var statusCode = firstError.Type switch
        {
            ErrorType.Failure => StatusCodes.Status400BadRequest,
            ErrorType.Unauthorized => StatusCodes.Status401Unauthorized,
            ErrorType.Forbidden => StatusCodes.Status403Forbidden,
            ErrorType.NotFound => StatusCodes.Status404NotFound,
            ErrorType.Validation => StatusCodes.Status406NotAcceptable,
            ErrorType.Conflict => StatusCodes.Status409Conflict,
            ErrorType.Unexpected => StatusCodes.Status503ServiceUnavailable,
            _ => StatusCodes.Status500InternalServerError
        };

        return Problem(
            statusCode: statusCode,
            title: firstError.Description);
    }

    /// <summary>
    /// Overrides the default ValidationProblem method to return a list of errors instead of a single error.
    /// </summary>
    /// <param name="errors">The list of errors to be added to the ModelStateDictionary.</param>
    /// <returns>An IActionResult representing the validation problem with the provided errors.</returns>
    /// 
    private IActionResult ValidationProblem(List<Error> errors)
    {
        var modelStateDictionary = new ModelStateDictionary();

        foreach (var error in errors)
        {
            modelStateDictionary.AddModelError(
                key: error.Code,
                errorMessage: error.Description);
        }

        return ValidationProblem(modelStateDictionary);
    }
}
