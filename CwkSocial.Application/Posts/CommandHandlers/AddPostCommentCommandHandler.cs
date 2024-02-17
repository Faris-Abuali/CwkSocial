using CwkSocial.Application.Models;
using CwkSocial.Application.Posts.Commands;
using CwkSocial.DataAccess;
using CwkSocial.Domain.Aggregates.PostAggregate;
using CwkSocial.Domain.Exceptions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Net;

namespace CwkSocial.Application.Posts.CommandHandlers;

public class AddPostCommentHandler : IRequestHandler<AddPostCommentCommand, OperationResult<PostComment>>
{
    private readonly DataContext _ctx;

    public AddPostCommentHandler(DataContext ctx)
    {
        _ctx = ctx;
    }
    public async Task<OperationResult<PostComment>> Handle(AddPostCommentCommand request, CancellationToken cancellationToken)
    {
        var result = new OperationResult<PostComment>();

        var post = await _ctx.Posts.FindAsync(request.PostId, cancellationToken);

        try
        {
            if (post is null)
            {
                result.IsError = true;
                result.Errors = [new Error { Message = $"No post found with id {request.PostId}" }];
                return result;
            }

            var comment = PostComment.Create(request.PostId, request.UserProfileId, request.CommentText);

            // Check the state of the comment entity
            // var entryC = _ctx.Entry(comment);
            // var stateC = entryC.State;

            post.AddComment(comment);

            /**
             * ℹ️ IMPORTANT: Explicitly set the state of the comment entity to Added
             * Otherwise, EF Core will consider the comment entity as Unchanged
             */
            _ctx.Entry(comment).State = EntityState.Added; // Attach the comment entity to the context

            // Update the post entity with the new comment
            _ctx.Posts.Update(post);

            // Save changes to the database
            await _ctx.SaveChangesAsync(cancellationToken);

            result.Payload = comment;
        }

        catch (PostCommentNotValidException ex)
        {
            result.IsError = true;

            result.Errors = ex.ValidationErrors
                .ConvertAll(errMessage => new Error
                {
                    Code = HttpStatusCode.BadRequest,
                    Message = errMessage,
                });
        }
        catch (Exception ex)
        {
            result.IsError = true;
            result.Errors = [new Error
            {
                Message = ex.InnerException?.Message ?? ex.Message,
                Code = HttpStatusCode.BadRequest
            }];
        }

        return result;
    }
}