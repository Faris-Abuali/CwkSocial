using CwkSocial.DataAccess;
using CwkSocial.Domain.Aggregates.PostAggregate;
using MediatR;
using Microsoft.EntityFrameworkCore;
using ErrorOr;
using CwkSocial.Domain.Common.Errors;

namespace CwkSocial.Application.Posts.GetAllPosts;

internal class GetAllPostsQueryHandler : IRequestHandler<GetAllPostsQuery, ErrorOr<IEnumerable<Post>>>
{
    private readonly DataContext _context;

    public GetAllPostsQueryHandler(DataContext context)
    {
        _context = context;
    }

    public async Task<ErrorOr<IEnumerable<Post>>> Handle(GetAllPostsQuery request, CancellationToken cancellationToken)
    {
        try
        {
            var posts = await _context.Posts.ToListAsync(cancellationToken);

            return posts;
        }
        catch (Exception ex)
        {
            return Errors.Unknown.Create(ex.Message);
        }
    }
}
