using ArticlesApp.Application.Abstractions.DataAccess;
using ArticlesApp.Application.DTOs.ArticleCategories;
using ArticlesApp.Domain.Entities;
using ArticlesApp.Domain.Errors;
using ArticlesApp.Domain.Result;
using MediatR;

namespace ArticlesApp.Application.CQRS.Queries.ArticleCategoryQueries.GetArticleCategory
{
    internal class GetArticleCategoryQueryHandler(IBaseRepository<ArticleCategory> repository) : IRequestHandler<GetArticleCategoryQuery, Result<ArticleCategoryResponseDTO>>
    {
        public async Task<Result<ArticleCategoryResponseDTO>> Handle(GetArticleCategoryQuery request, CancellationToken cancellationToken)
        {
            var articleCategoryId = request.Id;
           
            var articleCategory = await repository.GetByIdProjectedAsync<ArticleCategoryResponseDTO>(articleCategoryId, cancellationToken);

            if (articleCategory is null)
            {
                return Result<ArticleCategoryResponseDTO>.Failure([ArticleCategoryErrors.ArticleCategoryNotFound(articleCategoryId)]);
            }

            return Result<ArticleCategoryResponseDTO>.Success(articleCategory);
        }
    }
}
