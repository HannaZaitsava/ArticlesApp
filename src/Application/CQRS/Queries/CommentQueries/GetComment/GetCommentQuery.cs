using ArticlesApp.Application.Common.Caching;
using ArticlesApp.Application.DTOs.Comments;
using ArticlesApp.Domain.Result;
using MediatR;

namespace ArticlesApp.Application.CQRS.Queries.CommentQueries.GetComment
{
    public sealed record GetCommentQuery(Guid Id) : IRequest<Result<CommentResponseDTO>>, ICachableRequest
    {
        public string GetCacheKeyMetadata() => CacheKeys.Comment(Id);

        public IEnumerable<string>? CacheTags => [Common.Caching.CacheTags.Comment(Id), Common.Caching.CacheTags.Comments];
    }
}
