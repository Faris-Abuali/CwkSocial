using CwkSocial.DataAccess;
using CwkSocial.Domain.Aggregates.PostAggregate;
using CwkSocial.Domain.Common.Errors;
using ErrorOr;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CwkSocial.Application.Posts.GetPostComments;

internal class GetPostCommentsQueryHandler
    : IRequestHandler<GetPostCommentsQuery, ErrorOr<IEnumerable<PostComment>>>
{
    private readonly DataContext _context;

    public GetPostCommentsQueryHandler(DataContext context)
    {
        _context = context;
    }

    public async Task<ErrorOr<IEnumerable<PostComment>>> Handle(GetPostCommentsQuery request, CancellationToken cancellationToken)
    {
        try
        {
            var post = await _context.Posts
                .Include(p => p.Comments)
                .FirstOrDefaultAsync(p => p.PostId == request.PostId, cancellationToken: cancellationToken);

            if (post is null)
                return Errors.Post.PostNotFound;

            return post.Comments.ToList();
        }
        catch (Exception ex)
        {
            return Errors.Unknown.Create(ex.Message);
        }
    }
}
