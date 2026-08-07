using Application.CQRS.Commands.ArticleCommands.DeleteArticle;
using FluentValidation;

namespace Application.CQRS.Validators
{  
    public sealed class DeleteArticleCommandValidator : AbstractValidator<DeleteArticleCommand>
    {
        public DeleteArticleCommandValidator()
        {
            RuleFor(x => x.Id)
                .NotEmpty()
                .WithMessage("Article ID is required.");
        }
    }
}
