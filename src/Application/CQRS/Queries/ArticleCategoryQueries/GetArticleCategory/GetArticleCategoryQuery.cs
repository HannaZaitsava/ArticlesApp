using Application.Common.Caching;
using Application.DTOs.ArticleCategories;
using Domain.Result;
using MediatR;

namespace Application.CQRS.Queries.ArticleCategoryQueries.GetArticleCategory
{
    public sealed record GetArticleCategoryQuery(Guid Id) : IRequest<Result<ArticleCategoryResponseDTO>>, ICachableRequest
    {
        //public string CacheKey => CacheKeys.ArticleCategory(Id);
        public string GetCacheKeyMetadata() => CacheKeys.ArticleCategory(Id);

        public IEnumerable<string>? CacheTags => [Common.Caching.CacheTags.ArticleCategory(Id), Common.Caching.CacheTags.ArticleCategories];
    }
}
