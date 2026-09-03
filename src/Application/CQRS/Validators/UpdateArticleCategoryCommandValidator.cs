using ArticlesApp.Application.CQRS.Commands.ArticleCategoryCommands.UpdateArticleCategory;
using ArticlesApp.Domain.Constants.EntityConstraints;
using FluentValidation;

namespace ArticlesApp.Application.CQRS.Validators
{
    public sealed class UpdateArticleCategoryCommandValidator : AbstractValidator<UpdateArticleCategoryCommand>
    {
        public UpdateArticleCategoryCommandValidator()
        {
            RuleFor(x => x.Id)
                .NotEmpty()
                    .WithMessage("Category ID is required.");

            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Name is required")
                .Length(ArticleCategoryConstraints.NameMinLength, ArticleCategoryConstraints.NameMaxLength)
                   .WithMessage("Name must be between {MinLength} and {MaxLength} characters");                  
        }
    }
}
