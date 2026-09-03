using ArticlesApp.Application.Abstractions.DataAccess;
using ArticlesApp.Application.DTOs.ArticleCategories;
using ArticlesApp.Application.RequestFeatures.OffsetPagination;
using ArticlesApp.Domain.Result;
using MediatR;

namespace ArticlesApp.Application.CQRS.Queries.ArticleCategoryQueries.GetAllArticleCategories
{
    internal class GetAllArticleCategoriesQueryHandler(
        IArticleCategoryRepository categoryRepository)
        : IRequestHandler<GetAllArticleCategoriesQuery, Result<OffsetPagedResult<ArticleCategoryShotrInfoResponseDTO>>>
    {
        public async Task<Result<OffsetPagedResult<ArticleCategoryShotrInfoResponseDTO>>> Handle(GetAllArticleCategoriesQuery request, CancellationToken cancellationToken)
        {            
            var articleCategories = await categoryRepository.GetOffsetPagedListProjectedAsync<ArticleCategoryShotrInfoResponseDTO>(
                paginationParameters: request.PaginationParameters,
                cancellationToken);

            return Result<OffsetPagedResult<ArticleCategoryShotrInfoResponseDTO>>.Success(articleCategories); 
        }
    }
}
