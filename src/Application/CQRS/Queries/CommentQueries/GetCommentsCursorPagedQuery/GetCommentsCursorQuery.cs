using ArticlesApp.Application.Common.Caching;
using ArticlesApp.Application.Common.Constants;
using ArticlesApp.Application.DTOs.Comments;
using ArticlesApp.Application.Enums;
using ArticlesApp.Application.RequestFeatures.CursorPagination;
using ArticlesApp.Domain.Result;
using MediatR;

namespace ArticlesApp.Application.CQRS.Queries.CommentQueries.GetCommentsCursorPagedQuery
{    
    public sealed record GetCommentsCursorQuery
        : IRequest<Result<CursorPagedResult<CommentResponseDTO>>>,
          ICachableRequest
    {
        public Guid ArticleId { get; init; }

        public CursorPaginationParameters PaginationParameters { get; init; } = new()
        {
            Cursor = null,
            PageSize = PaginationConstants.CommentsDefaultPageSize,
            Direction = PaginationDirection.Forward
        };                
                
        public IEnumerable<string>? CacheTags => [Common.Caching.CacheTags.ArticleComments(ArticleId), Common.Caching.CacheTags.Comments];

        public string GetCacheKeyMetadata() =>
            $"article:{ArticleId}:cursor:{PaginationParameters.Cursor ?? "first"}:size:{PaginationParameters.PageSize}:dir:{PaginationParameters.Direction}";               
    }
}
