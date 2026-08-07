using Application.Common.Constants;
using Application.CQRS.Queries.ArticleQueries.GetAllArticlesQuery;
using Application.Enums.SortingEnums;
using Application.Extensions;
using Application.RequestFeatures.Sorting;
using Domain.Entities;
using FluentValidation;

namespace Application.CQRS.Validators
{    
    public class GetAllArticlesQueryValidator : AbstractValidator<GetAllArticlesQuery>
    {
        public GetAllArticlesQueryValidator()
        {
            RuleFor(x => x.PaginationParameters)
                .NotEmpty()
                // Передаем фабрику, которая создает и настраивает валидатор в рантайме
                .SetValidator(_ => new OffsetPaginationParametersValidator()
                    .Configure(PaginationConstants.ArticlesDefaultPageSize));

           //RuleFor(x => x.Sorts)
           //     .IsValidSortItemForEntity<GetAllArticlesQuery, ArticleSortItem, ArticleSortField, Article>();           
        }
    }
}
