﻿using CwkSocial.Api.Contracts.Common;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Net;

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
            var errors = new List<string>(); // Initialize an empty list

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

            context.Result = new JsonResult(response)
            {
                StatusCode = (int)HttpStatusCode.BadRequest
            };
        }
    }
}
