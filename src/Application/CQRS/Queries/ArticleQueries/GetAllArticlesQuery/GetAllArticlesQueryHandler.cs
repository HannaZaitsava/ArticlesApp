using ArticlesApp.Application.Abstractions.DataAccess;
using ArticlesApp.Application.DTOs.Articles;
using ArticlesApp.Application.RequestFeatures.OffsetPagination;
using ArticlesApp.Domain.Result;
using MediatR;

namespace ArticlesApp.Application.CQRS.Queries.ArticleQueries.GetAllArticlesQuery
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
