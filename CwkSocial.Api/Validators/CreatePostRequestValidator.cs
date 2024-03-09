using CwkSocial.Api.Contracts.Post.Requests;
using FluentValidation;

namespace CwkSocial.Api.Validators;

public class CreatePostRequestValidator : AbstractValidator<CreatePostRequest>
{
    public CreatePostRequestValidator()
    {
        RuleFor(x => x.TextContent)
            .Must(text => text.StartsWith('g'))
            .WithMessage("Text must start with 'g'. This is just to show the ability of FluentValidation");
    }
}
