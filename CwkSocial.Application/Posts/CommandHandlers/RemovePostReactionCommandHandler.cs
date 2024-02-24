using CwkSocial.Application.Models;
using CwkSocial.Application.Posts.Commands;
using CwkSocial.DataAccess;
using CwkSocial.Domain.Aggregates.PostAggregate;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Net;

namespace CwkSocial.Application.Posts.CommandHandlers;

internal class RemovePostReactionCommandHandler
    : IRequestHandler<RemovePostReactionCommand, OperationResult<PostReaction>>
{
    private readonly DataContext _context;

    public RemovePostReactionCommandHandler(DataContext context)
    {
        _context = context;
    }

    public async Task<OperationResult<PostReaction>> Handle(RemovePostReactionCommand request, CancellationToken cancellationToken)
    {
        var result = new OperationResult<PostReaction>();

        try
        {
            var post = await _context.Posts
            .Include(p => p.Reactions)
            .ThenInclude(r => r.UserProfile)
            .FirstOrDefaultAsync(p => p.PostId == request.PostId, cancellationToken);

            if (post is null)
            {
                result.AddError(
                        string.Format(PostsErrorMessages.PostNotFound, request.PostId),
                        HttpStatusCode.NotFound);

                return result;
            }

            var reaction = post.Reactions.FirstOrDefault(r => r.ReactionId == request.ReactionId);

            if (reaction is null)
            {
                result.AddError(
                         string.Format(PostsErrorMessages.ReactionNotFound, request.ReactionId),
                         HttpStatusCode.NotFound);

                return result;
            }

            if (reaction.UserProfileId != request.UserProfileId)
            {
                result.AddError(PostsErrorMessages.NotReactionOwner, HttpStatusCode.Forbidden);
                return result;
            }

            post.RemoveReaction(reaction);

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
