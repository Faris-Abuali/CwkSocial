using Microsoft.AspNetCore.Mvc;
using CwkSocial.Domain.Models;

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
        var post = new Post
        {
            Id = id,
            Text = "Hello, universe!"
        };

        return Ok(post);
    }
}
