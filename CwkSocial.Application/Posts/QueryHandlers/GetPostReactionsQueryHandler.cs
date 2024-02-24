
using CwkSocial.Application.Models;
using CwkSocial.Application.Posts.Queries;
using CwkSocial.DataAccess;
using CwkSocial.Domain.Aggregates.PostAggregate;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Net;

namespace CwkSocial.Application.Posts.QueryHandlers;

internal class GetPostReactionsQueryHandler
    : IRequestHandler<GetPostReactionsQuery, OperationResult<IEnumerable<PostReaction>>>
{
    private readonly DataContext _context;

    public GetPostReactionsQueryHandler(DataContext context)
    {
        _context = context;
    }

    public async Task<OperationResult<IEnumerable<PostReaction>>> Handle(GetPostReactionsQuery request, CancellationToken cancellationToken)
    {
        var result = new OperationResult<IEnumerable<PostReaction>>();

        try
        {
            var post = await _context.Posts
                .Where(p => p.PostId == request.PostId)
                .Include(p => p.Reactions) // fill in the Reactions property of the Post
                .ThenInclude(r => r.UserProfile) // fill in the UserProfile property of the PostReaction
                .FirstOrDefaultAsync(cancellationToken);

            if (post is null)
            {
                result.AddError(PostsErrorMessages.PostNotFound, HttpStatusCode.NotFound);
                return result;
            }

            result.Payload = post.Reactions.ToList();
        }
        catch (Exception e)
        {
            result.AddUnknownError(e.Message);
        }

        return result;
    }
}
