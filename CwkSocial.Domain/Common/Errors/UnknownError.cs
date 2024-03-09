using ErrorOr;

namespace CwkSocial.Domain.Common.Errors;

public static partial class Errors
{
    public static class Unknown
    {
        public static Error Create(Exception ex)
        {
            var metadata = new Dictionary<string, object>();

            if (ex.InnerException is not null)
                metadata.Add("innerException", ex.InnerException.Message);

            return Error.Unexpected(
              code: "UnknownError",
              description: ex.Message,
              metadata: metadata);
        }

        public static Error Create(string message) => Error.Unexpected(
              code: "UnknownError",
              description: message);
    }
}
