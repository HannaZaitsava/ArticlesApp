using Application.Common.Caching;
using Application.DTOs.Articles;
using Domain.Result;
using MediatR;

namespace Application.CQRS.Queries.ArticleQueries.GetArticleQuery
{
    public sealed record GetArticleQuery(Guid Id) : IRequest<Result<ArticleResponseDTO>>, ICachableRequest
    {
        public string GetCacheKeyMetadata() => CacheKeys.Article(Id);

        public IEnumerable<string>? CacheTags => [Common.Caching.CacheTags.Article(Id), Common.Caching.CacheTags.Articles, Common.Caching.CacheTags.ArticleCategories, Common.Caching.CacheTags.Comments];        
    }
}
