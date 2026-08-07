using Application.Abstractions.DataAccess;
using Application.DTOs.ArticleCategories;
using Domain.Entities;
using Domain.Errors;
using Domain.Result;
using MediatR;

namespace Application.CQRS.Queries.ArticleCategoryQueries.GetArticleCategory
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
