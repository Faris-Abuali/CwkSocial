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
        var userProfileId = HttpContext.GetUserProfileIdClaimValue();

        var command = new AddPostCommentCommand
        {
            PostId = Guid.Parse(postId),
            UserProfileId = userProfileId,
            CommentText = request.Text
        };

        var result = await _mediator.Send(command);

        if (result.IsError) return HandleErrorResponse(result.Errors);

        var response = _mapper.Map<PostCommentResponse>(result.Payload);

        return Ok(response);
    }

    [HttpGet]
    [Route(ApiRoutes.Posts.PostReactions)]
    [ValidateGuid("postId")]
    public async Task<IActionResult> GetPostReactions(string postId)
    {
        var postGuid = Guid.Parse(postId);

        var query = new GetPostReactionsQuery { PostId = postGuid };

        var result = await _mediator.Send(query);

        if (result.IsError) return HandleErrorResponse(result.Errors);

        var response = _mapper.Map<List<PostReactionResponse>>(result.Payload);

        return Ok(response);
    }

    /// <summary>
    /// Adds a reaction to a post
    /// </summary>
    /// <param name="postId">The post Id</param>
    /// <returns> if success</returns>
    /// <response code="200">Returns the newly added reaction</response>
    /// <response code="404">If the post is not found</response>
    [HttpPost]
    [Route(ApiRoutes.Posts.PostReactions)]
    [ValidateGuid("postId")]
    [ValidateModel]
    [ProducesResponseType(typeof(PostReactionResponse[]), StatusCodes.Status200OK)]
    public async Task<IActionResult> AddReactionToPost(string postId, [FromBody] CreatePostReactionRequest request)
    {
        var userProfileId = HttpContext.GetUserProfileIdClaimValue();

        var command = new AddPostReactionCommand
        {
            PostId = Guid.Parse(postId),
            UserProfileId = userProfileId,
            ReactionType = request.ReactionType,
        };

        var result = await _mediator.Send(command);

        if (result.IsError) return HandleErrorResponse(result.Errors);

        var response = _mapper.Map<PostReactionResponse>(result.Payload);

        return Ok(response);
    }
}
