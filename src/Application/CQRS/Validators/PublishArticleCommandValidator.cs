using Application.CQRS.Commands.ArticleCommands.PublishArticleCommand;
using FluentValidation;

namespace Application.CQRS.Validators
{    
    public sealed class PublishArticleCommandValidator : AbstractValidator<PublishArticleCommand>
    {
        public PublishArticleCommandValidator()
        {
            RuleFor(x => x.Id)
                .NotEmpty()
                .WithMessage("Article ID is required.");
        }
    }
}
