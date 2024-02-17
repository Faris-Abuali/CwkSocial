using CwkSocial.Application.Models;
using CwkSocial.Application.Posts.Queries;
using CwkSocial.DataAccess;
using CwkSocial.Domain.Aggregates.PostAggregate;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Net;

namespace CwkSocial.Application.Posts.QueryHandlers;

internal class GetPostCommentsQueryHandler
    : IRequestHandler<GetPostCommentsQuery, OperationResult<IEnumerable<PostComment>>>
{
    private readonly DataContext _context;

    public GetPostCommentsQueryHandler(DataContext context)
    {
        _context = context;
    }

    public async Task<OperationResult<IEnumerable<PostComment>>> Handle(GetPostCommentsQuery request, CancellationToken cancellationToken)
    {
        var result = new OperationResult<IEnumerable<PostComment>>();

        try
        {
            var post = await _context.Posts
                .Include(p => p.Comments)
                .FirstOrDefaultAsync(p => p.PostId == request.PostId, cancellationToken: cancellationToken);

            if (post is null)
            {
                result.Errors = [new Error
                {
                    Message = $"Post with id {request.PostId} not found",
                    Code = HttpStatusCode.NotFound
                }];

                return result;
            }

            result.Payload = post.Comments.ToList();
        }
        catch (Exception ex)
        {
            result.IsError = true;
            result.Errors = [new Error
            {
                Message = ex.Message,
                Code = HttpStatusCode.InternalServerError
            }];
            return result;
        }

        return result;
    }
}
