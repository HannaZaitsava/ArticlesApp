using Application.Common.Constants;
using Application.CQRS.Queries.ArticleCategoryQueries.GetAllArticleCategories;
using FluentValidation;

namespace Application.CQRS.Validators
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
