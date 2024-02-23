using CwkSocial.Application.Models;
using CwkSocial.Application.Posts.Commands;
using CwkSocial.Domain.Aggregates.PostAggregate;
using CwkSocial.Domain.Exceptions;
using MediatR;
using System.Net;
using CwkSocial.DataAccess;

namespace CwkSocial.Application.Posts.CommandHandlers;

internal class CreatePostCommandHandler
    : IRequestHandler<CreatePostCommand, OperationResult<Post>>
{
    private readonly DataContext _context;

    public CreatePostCommandHandler(DataContext context)
    {
        _context = context;
    }

    public async Task<OperationResult<Post>> Handle(CreatePostCommand request, CancellationToken cancellationToken)
    {
        var result = new OperationResult<Post>();

        try
        {
            // Create a new Post object
            var post = Post.Create(request.UserProfileId, request.TextContent);

            // Add the post to the database
            _context.Posts.Add(post);

            await _context.SaveChangesAsync(cancellationToken);

            result.Payload = post;
        }
        catch (PostNotValidException ex)
        {
            ex.ValidationErrors
                .ForEach(errMessage =>
                {
                    result.AddError(errMessage);
                });
        }
        catch (Exception ex)
        {
            result.AddUnknownError(ex.Message);
        }

        return result;
    }
}
