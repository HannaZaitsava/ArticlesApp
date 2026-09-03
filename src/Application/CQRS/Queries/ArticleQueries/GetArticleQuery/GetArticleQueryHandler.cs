using ArticlesApp.Application.Abstractions.DataAccess;
using ArticlesApp.Application.DTOs.Articles;
using ArticlesApp.Domain.Entities;
using ArticlesApp.Domain.Errors;
using ArticlesApp.Domain.Result;
using MediatR;

namespace ArticlesApp.Application.CQRS.Queries.ArticleQueries.GetArticleQuery
{
    internal class GetArticleQueryHandler(IBaseRepository<Article> repository) : IRequestHandler<GetArticleQuery, Result<ArticleResponseDTO>>
    {
        public async Task<Result<ArticleResponseDTO>> Handle(GetArticleQuery request, CancellationToken cancellationToken)
        {
            Guid articleId = request.Id;
            
            var articleResponseDTO = await repository.GetByIdProjectedAsync<ArticleResponseDTO>(articleId, cancellationToken);
                       
            if (articleResponseDTO is null)
            {
                return Result<ArticleResponseDTO>.Failure([ArticleErrors.ArticleNotFound(articleId)]);
            }

            return Result<ArticleResponseDTO>.Success(articleResponseDTO);
        }
    }
}
