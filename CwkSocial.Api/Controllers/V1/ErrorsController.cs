using Microsoft.AspNetCore.Mvc;

namespace CwkSocial.Api.Controllers.V1;

public class ErrorsController : ControllerBase
{
    [Route("error")]
    public IActionResult Error()
    {
        return Problem();
    }
}
