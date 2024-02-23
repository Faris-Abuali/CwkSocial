
using System.Net;

namespace CwkSocial.Application.Models;

public class Error
{
    public HttpStatusCode Code { get; set; } = HttpStatusCode.BadRequest;
    public string Message { get; set; } = string.Empty;
}
