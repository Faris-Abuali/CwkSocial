using CwkSocial.Application.Models;
using CwkSocial.DataAccess;
using CwkSocial.Domain.Aggregates.PostAggregate;
using CwkSocial.Domain.Common.Errors;
using CwkSocial.Domain.Exceptions;
using ErrorOr;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Net;

namespace CwkSocial.Application.Posts.AddPostComment;

public class AddPostCommentHandler : IRequestHandler<AddPostCommentCommand, ErrorOr<PostComment>>
{
    private readonly DataContext _ctx;

    public AddPostCommentHandler(DataContext ctx)
    {
        _ctx = ctx;
    }
    public async Task<ErrorOr<PostComment>> Handle(AddPostCommentCommand request, CancellationToken cancellationToken)
    {
        var post = await _ctx.Posts.FindAsync(request.PostId, cancellationToken);

        try
        {
            if (post is null)
                return Errors.Post.PostNotFound;

            var commentResult = PostComment.Create(request.PostId, request.UserProfileId, request.CommentText);

            if (commentResult.IsError)
                return commentResult.Errors;

            var comment = commentResult.Value;

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

            return comment;
        }
        //catch (PostCommentNotValidException ex)
        //{
        //    ex.ValidationErrors
        //        .ForEach(msg => result.AddError(msg));
        //}
        catch (Exception ex)
        {
            return Errors.Unknown.Create(ex.Message);
        }
    }
}