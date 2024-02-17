using CwkSocial.Domain.Aggregates.PostAggregate;
using FluentValidation;

namespace CwkSocial.Domain.Validators.PostsValidators;

public class PostCommentValidator : AbstractValidator<PostComment>
{
    public PostCommentValidator()
    {
        RuleFor(x => x.Text)
            .NotEmpty()
            .WithMessage("Comment text cannot be empty.")
            .MaximumLength(500)
            .WithMessage("Comment text cannot exceed 500 characters.")
            .MinimumLength(3)
            .WithMessage("Comment text must be at least 3 characters long.");
    }
}
