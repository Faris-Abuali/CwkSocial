
namespace CwkSocial.Application.Models;

public class OperationResult<T>
{
    public T? Payload { get; set; }

    // Make the IsError derived from the Errors list
    public bool IsError { get => Errors.Count > 0; set { } }

    public List<Error> Errors { get; set; } = [];
}
