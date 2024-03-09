using CwkSocial.Application.Models;
using CwkSocial.DataAccess;
using CwkSocial.Domain.Aggregates.PostAggregate;
using CwkSocial.Domain.Common.Errors;
using ErrorOr;
using MediatR;

namespace CwkSocial.Application.Posts.DeletePost;

internal class DeletePostCommandHandler
    : IRequestHandler<DeletePostCommand, ErrorOr<Post>>
{
    private readonly DataContext _context;

    public DeletePostCommandHandler(DataContext context)
    {
        _context = context;
    }

    public async Task<ErrorOr<Post>> Handle(DeletePostCommand request, CancellationToken cancellationToken)
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

            // Delete the post from the database
            _context.Posts.Remove(post);

            // Save the changes to the database
            await _context.SaveChangesAsync(cancellationToken);

            return post;
        }
        catch (Exception ex)
        {
            return Errors.Unknown.Create(ex.Message);
        }
    }
}
