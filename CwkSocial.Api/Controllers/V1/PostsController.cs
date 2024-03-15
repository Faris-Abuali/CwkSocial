using AutoMapper;
using CwkSocial.Api.Contracts.Post.Requests;
using CwkSocial.Api.Contracts.Post.Responses;
using CwkSocial.Api.Contracts.UserProfile.Responses;
using CwkSocial.Api.Filters;
using CwkSocial.Application.Posts.AddPostComment;
using CwkSocial.Application.Posts.AddPostReaction;
using CwkSocial.Application.Posts.CreatePost;
using CwkSocial.Application.Posts.DeletePost;
using CwkSocial.Application.Posts.GetAllPosts;
using CwkSocial.Application.Posts.GetPostById;
using CwkSocial.Application.Posts.GetPostComments;
using CwkSocial.Application.Posts.GetPostReactions;
using CwkSocial.Application.Posts.RemovePostReaction;
using CwkSocial.Application.Posts.UpdatePost;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;


namespace CwkSocial.Api.Controllers.V1;

[ApiVersion("1.0")]
[Route(ApiRoutes.BaseRoute)]
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

        return result.Match(
            posts => Ok(_mapper.Map<List<PostResponse>>(posts)),
            Problem);
    }

    [HttpGet]
    [Route(ApiRoutes.Posts.IdRoute)]
    [ValidateGuid("id")]
    public async Task<IActionResult> GetById(string id)
    {
        var postId = Guid.Parse(id);

        var query = new GetPostByIdQuery { PostId = postId };

        var result = await _mediator.Send(query);

        return result.Match(
            post => Ok(_mapper.Map<PostResponse>(post)),
            Problem);
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

        return result.Match(
            post =>
            {
                var response = _mapper.Map<PostResponse>(post);

                return CreatedAtAction(
                    nameof(GetById),
                    new { id = response.PostId },
                    response);
            },
            Problem);
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

        return result.Match(
            _ => NoContent(),
            Problem);
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

        return result.Match(
            _ => NoContent(),
            Problem);
    }

    [HttpGet]
    [Route(ApiRoutes.Posts.PostComments)]
    [ValidateGuid("postId")]
    public async Task<IActionResult> GetCommentsByPostId(string postId)
    {
        var query = new GetPostCommentsQuery { PostId = Guid.Parse(postId) };

        var result = await _mediator.Send(query);

        return result.Match(
         comments => Ok(_mapper.Map<List<PostCommentResponse>>(comments)),
         Problem);
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

        return result.Match(
             comment => Ok(_mapper.Map<PostCommentResponse>(comment)),
             Problem);
    }

    [HttpGet]
    [Route(ApiRoutes.Posts.PostReactions)]
    [ValidateGuid("postId")]
    public async Task<IActionResult> GetPostReactions(string postId)
    {
        var postGuid = Guid.Parse(postId);

        var query = new GetPostReactionsQuery { PostId = postGuid };

        var result = await _mediator.Send(query);

        return result.Match(
             reactions => Ok(_mapper.Map<List<PostReactionResponse>>(reactions)),
             Problem);
    }

    /// <summary>
    /// Adds a reaction to a post
    /// </summary>
    /// <param name="postId">The post Id</param>
    /// <param name="request"></param>
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

        return result.Match(
             reaction => Ok(_mapper.Map<PostReactionResponse>(reaction)),
             Problem);
    }

    [HttpDelete]
    [Route(ApiRoutes.Posts.ReactionById)]
    [ValidateGuid("postId")]
    [ValidateGuid("reactionId")]
    public async Task<IActionResult> RemovePostReaction(
        string postId,
        string reactionId)
    {
        var userProfileId = HttpContext.GetUserProfileIdClaimValue();

        var command = new RemovePostReactionCommand
        {
            PostId = Guid.Parse(postId),
            ReactionId = Guid.Parse(reactionId),
            UserProfileId = userProfileId
        };

        var result = await _mediator.Send(command);

        return result.Match(
             reaction => Ok(_mapper.Map<PostReactionResponse>(reaction)),
             Problem);
    }
}
