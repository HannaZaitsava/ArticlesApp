using ArticlesApp.Application.CQRS.Validators;
using ArticlesApp.Application.Common.Constants;
using ArticlesApp.Application.CQRS.Queries.ArticleCategoryQueries.GetAllArticleCategories;
using FluentValidation;

namespace ArticlesApp.Application.CQRS.Validators
{   
    public class GetAllArticleCategoriesQueryValidator : AbstractValidator<GetAllArticleCategoriesQuery>
    {
        public GetAllArticleCategoriesQueryValidator()
        {
            RuleFor(x => x.PaginationParameters)
                .NotEmpty()
                // Передаем фабрику, которая создает и настраивает валидатор «на лету»
                .SetValidator(_ => new OffsetPaginationParametersValidator()
                    .Configure(PaginationConstants.ArticleCategoriesDefaultPageSize));
        }
    }
}
