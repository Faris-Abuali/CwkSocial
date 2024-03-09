using Microsoft.AspNetCore.Mvc;

namespace CwkSocial.Api.Controllers.V2;

[ApiVersion("2.0")]
[Route("api/v{version:apiVersion}/[controller]")]
[ApiController]
public class PostsController : ControllerBase
{
    [HttpGet]
    [Route("{id}")]
    public IActionResult GetById(int id)
    {
        return Ok(new { message = "Hello Universe"});
    }
}
