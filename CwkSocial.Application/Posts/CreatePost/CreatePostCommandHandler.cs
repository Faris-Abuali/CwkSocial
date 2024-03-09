using CwkSocial.Domain.Common.Errors;
using CwkSocial.Domain.Aggregates.PostAggregate;
using MediatR;
using CwkSocial.DataAccess;
using ErrorOr;

namespace CwkSocial.Application.Posts.CreatePost;

internal class CreatePostCommandHandler
    : IRequestHandler<CreatePostCommand, ErrorOr<Post>>
{
    private readonly DataContext _context;

    public CreatePostCommandHandler(DataContext context)
    {
        _context = context;
    }

    public async Task<ErrorOr<Post>> Handle(CreatePostCommand request, CancellationToken cancellationToken)
    {
        try
        {
            // Create a new Post object
            var postResult = Post.Create(request.UserProfileId, request.TextContent);

            if (postResult.IsError)
                return postResult.Errors;

            var post = postResult.Value;

            // Add the post to the database
            _context.Posts.Add(post);

            await _context.SaveChangesAsync(cancellationToken);

            return post;
    }
        catch (Exception ex)
        {
            return Errors.Unknown.Create(ex);
        }
    }
}
