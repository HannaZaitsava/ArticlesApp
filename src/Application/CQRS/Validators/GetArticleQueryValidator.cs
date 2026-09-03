using ArticlesApp.Application.CQRS.Queries.ArticleQueries.GetArticleQuery;
using FluentValidation;

namespace ArticlesApp.Application.CQRS.Validators
{
    public sealed class GetArticleQueryValidator : AbstractValidator<GetArticleQuery>
    {
        public GetArticleQueryValidator()
        {
            RuleFor(x => x.Id)
                .NotEmpty().WithMessage("Article Id is required");
        }
    }
}
