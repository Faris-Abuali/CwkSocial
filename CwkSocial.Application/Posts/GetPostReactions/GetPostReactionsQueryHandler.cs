using CwkSocial.DataAccess;
using CwkSocial.Domain.Aggregates.PostAggregate;
using MediatR;
using Microsoft.EntityFrameworkCore;
using ErrorOr;
using CwkSocial.Domain.Common.Errors;

namespace CwkSocial.Application.Posts.GetPostReactions;

internal class GetPostReactionsQueryHandler
    : IRequestHandler<GetPostReactionsQuery, ErrorOr<IEnumerable<PostReaction>>>
{
    private readonly DataContext _context;

    public GetPostReactionsQueryHandler(DataContext context)
    {
        _context = context;
    }

    public async Task<ErrorOr<IEnumerable<PostReaction>>> Handle(GetPostReactionsQuery request, CancellationToken cancellationToken)
    {
        try
        {
            var post = await _context.Posts
                .Where(p => p.PostId == request.PostId)
                .Include(p => p.Reactions) // fill in the Reactions property of the Post
                .ThenInclude(r => r.UserProfile) // fill in the UserProfile property of the PostReaction
                .FirstOrDefaultAsync(cancellationToken);

            if (post is null)
                return Errors.Post.PostNotFound;

            return post.Reactions.ToList();
        }
        catch (Exception ex)
        {
            return Errors.Unknown.Create(ex.Message);
        }
    }
}
