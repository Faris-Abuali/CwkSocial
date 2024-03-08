using CwkSocial.Api.Contracts.Common;
using ErrorOr;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.Net;
using System.Text.Json;

namespace CwkSocial.Api.Filters;

public class ValidateModelAttribute : ActionFilterAttribute
{

    /// <summary>
    /// This method is called before the action method is invoked.
    /// It checks if the model state is valid and if not, it returns a BadRequest response.
    /// </summary>
    /// <param name="context"></param>
    public override void OnResultExecuting(ResultExecutingContext context)
    {
        if (!context.ModelState.IsValid)
        {
            //var errors = new SerializableError();

            //foreach (var modelStateEntry in context.ModelState)
            //{
            //    foreach (var error in modelStateEntry.Value.Errors)
            //    {
            //        errors.Add(modelStateEntry.Key, error.ErrorMessage);
            //    }
            //}

            context.Result = new BadRequestObjectResult(
                new ValidationProblemDetails(context.ModelState));
        }
    }
}
