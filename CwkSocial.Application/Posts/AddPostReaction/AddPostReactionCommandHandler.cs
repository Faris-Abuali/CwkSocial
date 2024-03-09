using CwkSocial.DataAccess;
using CwkSocial.Domain.Aggregates.PostAggregate;
using CwkSocial.Domain.Common.Errors;
using ErrorOr;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CwkSocial.Application.Posts.AddPostReaction;

internal class AddPostReactionCommandHandler
    : IRequestHandler<AddPostReactionCommand, ErrorOr<PostReaction>>
{
    private readonly DataContext _context;

    public AddPostReactionCommandHandler(DataContext context)
    {
        _context = context;
    }

    public async Task<ErrorOr<PostReaction>> Handle(AddPostReactionCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var post = await _context.Posts
                .Include(p => p.Reactions)
                .FirstOrDefaultAsync(p => p.PostId == request.PostId, cancellationToken);

            if (post is null)
                return Errors.Post.PostNotFound;


            var reaction = PostReaction.Create(request.PostId, request.UserProfileId, request.ReactionType);

            _context.Entry(reaction).State = EntityState.Added; // Explicitly Attach the reaction entity to the context

            // Include the user profile in the reaction entity
            _context.Entry(reaction).Reference(r => r.UserProfile).Load();

            post.AddReaction(reaction);

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
