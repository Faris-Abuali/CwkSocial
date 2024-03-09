using CwkSocial.DataAccess;
using CwkSocial.Domain.Aggregates.PostAggregate;
using CwkSocial.Domain.Common.Errors;
using ErrorOr;
using MediatR;

namespace CwkSocial.Application.Posts.GetPostById;

internal class GetPostByIdQueryHandler : IRequestHandler<GetPostByIdQuery, ErrorOr<Post>>
{
    private readonly DataContext _context;

    public GetPostByIdQueryHandler(DataContext context)
    {
        _context = context;
    }

    public async Task<ErrorOr<Post>> Handle(GetPostByIdQuery request, CancellationToken cancellationToken)
    {
        var post = await _context.Posts.FindAsync(request.PostId);

        if (post is null)
            return Errors.Post.PostNotFound;

        return post;
    }
}
