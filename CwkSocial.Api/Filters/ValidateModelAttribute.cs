using CwkSocial.Api.Contracts.Common;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Net;

namespace CwkSocial.Api.Filters;

public class ValidateModelAttribute : ActionFilterAttribute
{
    public override void OnResultExecuting(ResultExecutingContext context)
    {
        if (!context.ModelState.IsValid)
        {
            var errors = new List<string>(); // Initialize an empty list

            Console.WriteLine(context.ModelState);
            foreach (var modelStateEntry in context.ModelState)
            {
                foreach (var error in modelStateEntry.Value.Errors)
                {
                    errors.Add(error.ErrorMessage);
                }
            }

            var response = new ErrorResponse
            {
                Phrase = "Bad Request",
                Code = (int)HttpStatusCode.BadRequest,
                Timestamp = DateTime.UtcNow,
                Errors = errors
            };

            // TODO: Make sure Asp.Net Core doesn't override our action result body
            context.Result = new JsonResult(response);
        }
    }
}
