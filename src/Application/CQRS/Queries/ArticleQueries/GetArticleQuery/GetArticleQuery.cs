using ArticlesApp.Application.Common.Caching;
using ArticlesApp.Application.DTOs.Articles;
using ArticlesApp.Domain.Result;
using MediatR;

namespace ArticlesApp.Application.CQRS.Queries.ArticleQueries.GetArticleQuery
{
    public sealed record GetArticleQuery(Guid Id) : IRequest<Result<ArticleResponseDTO>>, ICachableRequest
    {
        public string GetCacheKeyMetadata() => CacheKeys.Article(Id);

        public IEnumerable<string>? CacheTags => [Common.Caching.CacheTags.Article(Id), Common.Caching.CacheTags.Articles];        
    }
}
