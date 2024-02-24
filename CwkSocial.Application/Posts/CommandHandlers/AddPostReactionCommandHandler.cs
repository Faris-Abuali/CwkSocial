using CwkSocial.Application.Models;
using CwkSocial.Application.Posts.Commands;
using CwkSocial.DataAccess;
using CwkSocial.Domain.Aggregates.PostAggregate;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Net;

namespace CwkSocial.Application.Posts.CommandHandlers;

internal class AddPostReactionCommandHandler
    : IRequestHandler<AddPostReactionCommand, OperationResult<PostReaction>>
{
    private readonly DataContext _context;

    public AddPostReactionCommandHandler(DataContext context)
    {
        _context = context;
    }

    public async Task<OperationResult<PostReaction>> Handle(AddPostReactionCommand request, CancellationToken cancellationToken)
    {
        var result = new OperationResult<PostReaction>();

        try
        {
            var post = await _context.Posts
                .Include(p => p.Reactions)
                .FirstOrDefaultAsync(p => p.PostId == request.PostId, cancellationToken);

            if (post is null)
            {
                result.AddError(
                    string.Format(PostsErrorMessages.PostNotFound, request.PostId),
                    HttpStatusCode.NotFound);

                return result;
            }

            var reaction = PostReaction.Create(request.PostId, request.UserProfileId, request.ReactionType);

            _context.Entry(reaction).State = EntityState.Added; // Explicitly Attach the reaction entity to the context

            // Include the user profile in the reaction entity
            _context.Entry(reaction).Reference(r => r.UserProfile).Load();

            post.AddReaction(reaction);

            _context.Posts.Update(post);

            await _context.SaveChangesAsync(cancellationToken);

            result.Payload = reaction;
        }
        catch (Exception ex)
        {
            result.AddUnknownError(ex.Message);
        }

        return result;
    }
}
