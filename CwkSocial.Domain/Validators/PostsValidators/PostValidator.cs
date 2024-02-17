using CwkSocial.Domain.Aggregates.PostAggregate;
using FluentValidation;

namespace CwkSocial.Domain.Validators.PostsValidators;

public class PostValidator : AbstractValidator<Post>
{
    public PostValidator()
    {
        RuleFor(x => x.TextContent)
            .NotEmpty()
            .WithMessage("Post text cannot be empty.")
            .MaximumLength(500)
            .WithMessage("Post text cannot exceed 500 characters.")
            .MinimumLength(3)
            .WithMessage("Post text must be at least 3 characters long.");
    }
}
