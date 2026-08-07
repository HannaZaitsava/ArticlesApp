using Application.Abstractions.DataAccess;
using Application.DTOs.Articles;
using Application.RequestFeatures.OffsetPagination;
using Domain.Result;
using MediatR;

namespace Application.CQRS.Queries.ArticleQueries.GetAllArticlesQuery
{
    internal class GetAllArticlesQueryHandler(IArticleRepository articleRepository) : 
        IRequestHandler<GetAllArticlesQuery, 
            Result<OffsetPagedResult<ArticleShortInfoResponseDTO>>>
    {        
        public async Task<Result<OffsetPagedResult<ArticleShortInfoResponseDTO>>> Handle(GetAllArticlesQuery request, CancellationToken cancellationToken)
        {    
            var articles = await articleRepository.GetArticlesOffsetPagedListProjectedAsync<ArticleShortInfoResponseDTO>(
                sort: request.Sorts,
                paginationParameters: request.PaginationParameters,
                ct: cancellationToken);
            
            return Result<OffsetPagedResult<ArticleShortInfoResponseDTO>>.Success(articles);
        }
    }
}
