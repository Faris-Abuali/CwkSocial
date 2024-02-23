
using System.Net;

namespace CwkSocial.Application.Models;

public class OperationResult<T>
{
    public T? Payload { get; set; }

    public bool IsError { get => Errors.Count > 0; }

    public List<Error> Errors { get; private set; } = [];

    /// <summary>
    /// Add an error to the Errors list
    /// </summary>
    /// <param name="message"></param>
    /// <param name="code"></param>
    public void AddError(string message, HttpStatusCode code = HttpStatusCode.BadRequest)
    {
        HandleError(message, code);
    }

    /// <summary>
    /// Adds an error to the Errors list with an unknown status code
    /// </summary>
    /// <param name="message"></param>
    /// <param name="code"></param>
    public void AddUnknownError(string message)
    {
        HandleError(message, HttpStatusCode.InternalServerError);
    }

    public void ResetErrors() => Errors.Clear();

    #region Private Methods
    private void HandleError(string message, HttpStatusCode code)
    {
        Errors.Add(new Error { Code = code, Message = message });
    }
    #endregion

}
