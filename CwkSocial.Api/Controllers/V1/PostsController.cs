using AutoMapper;
using CwkSocial.Api.Contracts.Post.Requests;
using CwkSocial.Api.Contracts.Post.Responses;
using CwkSocial.Api.Contracts.UserProfile.Responses;
using CwkSocial.Api.Filters;
using CwkSocial.Application.Posts.Commands;
using CwkSocial.Application.Posts.Queries;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CwkSocial.Api.Controllers.V1;

[ApiVersion("1.0")]
[Route(ApiRoutes.BaseRoute)]
[ApiController]
[Authorize()]
public class PostsController : ApiController
{
    private readonly ISender _mediator;
    private readonly IMapper _mapper;

    public PostsController(ISender mediator, IMapper mapper)
    {
        _mediator = mediator;
        _mapper = mapper;
    }

    [HttpGet]
    public async Task<IActionResult> GetAllPosts()
    {
        var result = await _mediator.Send(new GetAllPostsQuery());

        if (result.IsError) return HandleErrorResponse(result.Errors);

        var response = _mapper.Map<List<PostResponse>>(result.Payload);

        return Ok(response);
    }

    [HttpGet]
    [Route(ApiRoutes.Posts.IdRoute)]
    [ValidateGuid("id")]
    public async Task<IActionResult> GetById(string id)
    {
        var postId = Guid.Parse(id);

        var query = new GetPostByIdQuery { PostId = postId };

        var result = await _mediator.Send(query);

        if (result.IsError) return HandleErrorResponse(result.Errors);

        var response = _mapper.Map<PostResponse>(result.Payload);

        return Ok(response);
    }

    [HttpPost]
    [ValidateModel]
    public async Task<IActionResult> CreatePost([FromBody] CreatePostRequest request)
    {
        // Get the UserProfileId from the token claims
        var userProfileId = HttpContext.GetUserProfileIdClaimValue();

        var command = new CreatePostCommand
        {
            UserProfileId = userProfileId,
            TextContent = request.TextContent
        };

        var result = await _mediator.Send(command);

        if (result.IsError) return HandleErrorResponse(result.Errors);

        var response = _mapper.Map<PostResponse>(result.Payload);

        return CreatedAtAction(
            nameof(GetById),
            new { id = response.PostId },
            response);
    }

    [HttpPut]
    [ValidateModel]
    [Route(ApiRoutes.Posts.IdRoute)]
    [ValidateGuid("id")]
    public async Task<IActionResult> UpdatePost(string id, [FromBody] UpdatePostRequest request)
    {
        // Get the UserProfileId from the token claims
        var userProfileId = HttpContext.GetUserProfileIdClaimValue();

        var postId = Guid.Parse(id);

        var command = new UpdatePostCommand
        {
            TextContent = request.TextContent,
            PostId = postId,
            UserProfileId = userProfileId
        };

        var result = await _mediator.Send(command);

        return result.IsError
            ? HandleErrorResponse(result.Errors)
            : NoContent();
    }

    [HttpDelete]
    [Route(ApiRoutes.Posts.IdRoute)]
    [ValidateGuid("id")]
    public async Task<IActionResult> DeletePost(string id)
    {
        // Get the UserProfileId from the token claims
        var userProfileId = HttpContext.GetUserProfileIdClaimValue();

        var command = new DeletePostCommand
        {
            PostId = Guid.Parse(id),
            UserProfileId = userProfileId
        };

        var result = await _mediator.Send(command);

        return result.IsError
            ? HandleErrorResponse(result.Errors)
            : NoContent();
    }

    [HttpGet]
    [Route(ApiRoutes.Posts.PostComments)]
    [ValidateGuid("postId")]
    public async Task<IActionResult> GetCommentsByPostId(string postId)
    {
        var query = new GetPostCommentsQuery { PostId = Guid.Parse(postId) };

        var result = await _mediator.Send(query);

        if (result.IsError) return HandleErrorResponse(result.Errors);

        var response = _mapper.Map<List<PostCommentResponse>>(result.Payload);

        return Ok(response);
    }

    [HttpPost]
    [Route(ApiRoutes.Posts.PostComments)]
    [ValidateGuid("postId")]
    [ValidateModel]
    public async Task<IActionResult> AddCommentToPost(string postId, [FromBody] CreatePostCommentRequest request)
    {
        var command = new AddPostCommentCommand
        {
            PostId = Guid.Parse(postId),
            UserProfileId = request.UserProfileId,
            CommentText = request.Text
        };

        var result = await _mediator.Send(command);

        if (result.IsError) return HandleErrorResponse(result.Errors);

        var response = _mapper.Map<PostCommentResponse>(result.Payload);

        return Ok(response);

        //return CreatedAtAction(
        //         nameof(GetCommentsByPostId),
        //         new { postId = response.PostId },
        //         response);
    }
}
