using Application.CQRS.Queries.ArticleCategoryQueries.GetArticleCategory;
using FluentValidation;

namespace Application.CQRS.Validators
{
    public sealed class GetArticleCategoryQueryValidator : AbstractValidator<GetArticleCategoryQuery>
    {
        public GetArticleCategoryQueryValidator()
        {
            RuleFor(x => x.Id)
                .NotEmpty().WithMessage("Article category Id is required");
        }
    }
}
