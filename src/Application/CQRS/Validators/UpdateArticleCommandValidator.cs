using Application.CQRS.Commands.ArticleCommands.UpdateArticle;
using Domain.Constants.EntityConstraints;
using FluentValidation;

namespace Application.CQRS.Validators
{
    public sealed class UpdateArticleCommandValidator : AbstractValidator<UpdateArticleCommand>
    {
        public UpdateArticleCommandValidator()
        {
            RuleFor(x => x.Id)
                .NotEmpty().WithMessage("Article Id is required");            

            RuleFor(x => x.Title)
                   .NotEmpty().WithMessage("Title is required")
                   .Length(ArticleConstraints.TitleMinLength, ArticleConstraints.TitleMaxLength)
                   .WithMessage("Title must be between {MinLength} and {MaxLength} characters");

            RuleFor(x => x.Content)
               .NotEmpty().WithMessage("Content is required")
               .Length(ArticleConstraints.ContentMinLength, ArticleConstraints.ContentMaxLength)
               .WithMessage("Content must be between {MinLength} and {MaxLength} characters");
        }
    }
}
