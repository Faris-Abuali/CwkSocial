
using CwkSocial.Application.Models;
using CwkSocial.DataAccess;
using CwkSocial.Domain.Aggregates.PostAggregate;
using CwkSocial.Domain.Exceptions;
using MediatR;
using ErrorOr;
using CwkSocial.Domain.Common.Errors;

namespace CwkSocial.Application.Posts.UpdatePost;

internal class UpdatePostCommandHandler
    : IRequestHandler<UpdatePostCommand, ErrorOr<Post>>
{
    private readonly DataContext _context;

    public UpdatePostCommandHandler(DataContext context)
    {
        _context = context;
    }

    public async Task<ErrorOr<Post>> Handle(UpdatePostCommand request, CancellationToken cancellationToken)
    {
        try
        {
            // Find the post in the database
            var post = await _context.Posts.FindAsync(request.PostId);

            if (post is null)
                return Errors.Post.PostNotFound;

            // Check if the user is the owner of the post
            if (post.UserProfileId != request.UserProfileId)
                return Errors.Post.NotPostOwner(request.PostId.ToString());

            // Update the post's text content
            post.UpdatePostText(request.TextContent);

            // Update the post in the database
            _context.Posts.Update(post);

            // Save the changes to the database
            await _context.SaveChangesAsync(cancellationToken);

            return post;
        }
        //catch (PostNotValidException ex)
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
