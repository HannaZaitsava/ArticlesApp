using Application.Abstractions.DataAccess;
using Application.DTOs.ArticleCategories;
using Application.RequestFeatures.OffsetPagination;
using Domain.Result;
using MediatR;

namespace Application.CQRS.Queries.ArticleCategoryQueries.GetAllArticleCategories
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
