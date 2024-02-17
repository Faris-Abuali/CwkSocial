
using CwkSocial.Application.Models;
using CwkSocial.Application.Posts.Queries;
using CwkSocial.DataAccess;
using CwkSocial.Domain.Aggregates.PostAggregate;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CwkSocial.Application.Posts.QueryHandlers;

internal class GetAllPostsQueryHandler : IRequestHandler<GetAllPostsQuery, OperationResult<IEnumerable<Post>>>
{
    private readonly DataContext _context;

    public GetAllPostsQueryHandler(DataContext context)
    {
        _context = context;
    }

    public async Task<OperationResult<IEnumerable<Post>>> Handle(GetAllPostsQuery request, CancellationToken cancellationToken)
    {
        var result = new OperationResult<IEnumerable<Post>>();

        try
        {
            var posts = await _context.Posts.ToListAsync(cancellationToken);

            result.Payload = posts;
        }
        catch (Exception ex)
        {
            result.IsError = true;
            result.Errors = [new Error { Message = ex.Message }];
        }

        return result;
    }
}
