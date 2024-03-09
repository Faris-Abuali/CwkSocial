using CwkSocial.DataAccess;
using CwkSocial.Domain.Aggregates.PostAggregate;
using MediatR;
using Microsoft.EntityFrameworkCore;
using ErrorOr;
using CwkSocial.Domain.Common.Errors;

namespace CwkSocial.Application.Posts.RemovePostReaction;

internal class RemovePostReactionCommandHandler
    : IRequestHandler<RemovePostReactionCommand, ErrorOr<PostReaction>>
{
    private readonly DataContext _context;

    public RemovePostReactionCommandHandler(DataContext context)
    {
        _context = context;
    }

    public async Task<ErrorOr<PostReaction>> Handle(RemovePostReactionCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var post = await _context.Posts
            .Include(p => p.Reactions)
            .ThenInclude(r => r.UserProfile)
            .FirstOrDefaultAsync(p => p.PostId == request.PostId, cancellationToken);

            if (post is null)
                return Errors.Post.PostNotFound;


            var reaction = post.Reactions.FirstOrDefault(r => r.ReactionId == request.ReactionId);

            if (reaction is null)
                return Errors.Post.ReactionNotFound;


            if (reaction.UserProfileId != request.UserProfileId)
                return Errors.Post.NotReactionOwner(request.ReactionId.ToString());

            post.RemoveReaction(reaction);

            _context.Posts.Update(post);

            await _context.SaveChangesAsync(cancellationToken);

            return reaction;
        }
        catch (Exception ex)
        {
            return Errors.Unknown.Create(ex.Message);
        }
    }
}
