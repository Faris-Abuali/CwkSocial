
using CwkSocial.Application.Models;
using CwkSocial.Application.Posts.Queries;
using CwkSocial.DataAccess;
using CwkSocial.Domain.Aggregates.PostAggregate;
using CwkSocial.Domain.Aggregates.UserProfileAggregate;
using MediatR;
using System.Net;

namespace CwkSocial.Application.Posts.QueryHandlers;

internal class GetPostByIdQueryHandler : IRequestHandler<GetPostByIdQuery, OperationResult<Post>>
{
    private readonly DataContext _context;

    public GetPostByIdQueryHandler(DataContext context)
    {
        _context = context;
    }

    public async Task<OperationResult<Post>> Handle(GetPostByIdQuery request, CancellationToken cancellationToken)
    {
        var result = new OperationResult<Post>();

        var post = await _context.Posts.FindAsync(request.PostId);

        if (post is null)
        {
            result.IsError = true;
            result.Errors = [new Error
            {
                Code = HttpStatusCode.NotFound,
                Message = $"No Post found with ID: {request.PostId}"
            }];
            return result;
        }

        result.Payload = post;
        return result;
    }
}
