using Microsoft.AspNetCore.Mvc;

namespace CwkSocial.Api.Controllers.V1;

[ApiVersion("1.0")]
[Route(ApiRoutes.BaseRoute)]
[ApiController]
public class PostsController : ControllerBase
{
    [HttpGet]
    [Route(ApiRoutes.Posts.GetById)]
    public IActionResult GetById(int id)
    {
        // var post = new Post
        // {
        //     Id = id,
        //     Text = "Hello, World!"
        // };

        return Ok();
    }
}
