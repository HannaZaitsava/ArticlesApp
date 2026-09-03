using ArticlesApp.Application.Common.Caching;
using ArticlesApp.Application.DTOs.Tags;
using ArticlesApp.Domain.Result;
using MediatR;

namespace ArticlesApp.Application.CQRS.Queries.TagQueries.GetTag
{   
    public sealed record GetTagQuery(Guid Id) : IRequest<Result<TagResponseDTO>>, ICachableRequest
    {
        public string GetCacheKeyMetadata() => CacheKeys.Tag(Id);

        public IEnumerable<string>? CacheTags => [Common.Caching.CacheTags.Tag(Id), Common.Caching.CacheTags.Tags];
    }
}
