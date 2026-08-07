using Application.Common.Caching;
using Application.DTOs.Comments;
using Domain.Result;
using MediatR;

namespace Application.CQRS.Queries.CommentQueries.GetComment
{
    public sealed record GetCommentQuery(Guid Id) : IRequest<Result<CommentResponseDTO>>, ICachableRequest
    {
        public string GetCacheKeyMetadata() => CacheKeys.Comment(Id);

        public IEnumerable<string>? CacheTags => [Common.Caching.CacheTags.Comment(Id), Common.Caching.CacheTags.Comments];
    }
}
