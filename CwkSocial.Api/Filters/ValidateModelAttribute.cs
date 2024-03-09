using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

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
