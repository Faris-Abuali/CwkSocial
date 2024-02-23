
using CwkSocial.Application.Models;
using CwkSocial.Application.Posts.Commands;
using CwkSocial.DataAccess;
using CwkSocial.Domain.Aggregates.PostAggregate;
using MediatR;
using System.Net;

namespace CwkSocial.Application.Posts.CommandHandlers;

internal class DeletePostCommandHandler
    : IRequestHandler<DeletePostCommand, OperationResult<Post>>
{
    private readonly DataContext _context;

    public DeletePostCommandHandler(DataContext context)
    {
        _context = context;
    }

    public async Task<OperationResult<Post>> Handle(DeletePostCommand request, CancellationToken cancellationToken)
    {
        var result = new OperationResult<Post>();

        try
        {
            // Find the post in the database
            var post = await _context.Posts.FindAsync(request.PostId);

            if (post is null)
            {
                result.AddError(
                    string.Format(PostsErrorMessages.PostNotFound, request.PostId),
                    HttpStatusCode.NotFound);

                return result;
            }

            // Check if the user is the owner of the post
            if (post.UserProfileId != request.UserProfileId)
            {
                result.AddError(PostsErrorMessages.NotPostOwner, HttpStatusCode.Forbidden);
                return result;
            }

            // Delete the post from the database
            _context.Posts.Remove(post);

            // Save the changes to the database
            await _context.SaveChangesAsync(cancellationToken);

            result.Payload = post;
        }
        catch (Exception ex)
        {
            result.AddUnknownError(ex.Message);
        }

        return result;
    }
}
