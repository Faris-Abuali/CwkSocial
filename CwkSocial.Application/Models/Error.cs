
using System.Net;

namespace CwkSocial.Application.Models;

public class Error
{
    public HttpStatusCode Code { get; set; }
    public string Message { get; set; } = string.Empty;
}
