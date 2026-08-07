using Application.CQRS.Commands.ArticleCategoryCommands.DeleteArticleCategory;
using FluentValidation;

namespace Application.CQRS.Validators
{    
    public sealed class DeleteArticleCategoryCommandValidator : AbstractValidator<DeleteArticleCategoryCommand>
    {
        public DeleteArticleCategoryCommandValidator()
        {
            RuleFor(x => x.Id)
                .NotEmpty() 
                .WithMessage("Article category ID is required.");
        }
    }
}
