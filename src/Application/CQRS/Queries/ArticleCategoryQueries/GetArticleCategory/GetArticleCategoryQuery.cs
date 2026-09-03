using ArticlesApp.Application.Common.Caching;
using ArticlesApp.Application.DTOs.ArticleCategories;
using ArticlesApp.Domain.Result;
using MediatR;

namespace ArticlesApp.Application.CQRS.Queries.ArticleCategoryQueries.GetArticleCategory
{
    public sealed record GetArticleCategoryQuery(Guid Id) : IRequest<Result<ArticleCategoryResponseDTO>>, ICachableRequest
    {
        public string GetCacheKeyMetadata() => CacheKeys.ArticleCategory(Id);

        public IEnumerable<string>? CacheTags => [Common.Caching.CacheTags.ArticleCategory(Id), Common.Caching.CacheTags.ArticleCategories];
    }
}
