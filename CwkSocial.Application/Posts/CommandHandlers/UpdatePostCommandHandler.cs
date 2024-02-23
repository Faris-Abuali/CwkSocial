
using CwkSocial.Application.Models;
using CwkSocial.Application.Posts.Commands;
using CwkSocial.DataAccess;
using CwkSocial.Domain.Aggregates.PostAggregate;
using CwkSocial.Domain.Exceptions;
using MediatR;
using System.Net;

namespace CwkSocial.Application.Posts.CommandHandlers;

internal class UpdatePostCommandHandler
    : IRequestHandler<UpdatePostCommand, OperationResult<Post>>
{
    private readonly DataContext _context;

    public UpdatePostCommandHandler(DataContext context)
    {
        _context = context;
    }

    public async Task<OperationResult<Post>> Handle(UpdatePostCommand request, CancellationToken cancellationToken)
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

            // Update the post's text content
            post.UpdatePostText(request.TextContent);

            // Update the post in the database
            _context.Posts.Update(post);

            // Save the changes to the database
            await _context.SaveChangesAsync(cancellationToken);

            result.Payload = post;
        }
        catch (PostNotValidException ex)
        {
            ex.ValidationErrors
                .ForEach(msg => result.AddError(msg));
        }
        catch (Exception ex)
        {
            result.AddUnknownError(ex.Message);
        }

        return result;
    }
}
