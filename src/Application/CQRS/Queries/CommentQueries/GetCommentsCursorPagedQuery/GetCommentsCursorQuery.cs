using Application.Common.Caching;
using Application.Common.Constants;
using Application.DTOs.Comments;
using Application.Enums;
using Application.RequestFeatures.CursorPagination;
using Domain.Result;
using MediatR;

namespace Application.CQRS.Queries.CommentQueries.GetCommentsCursorPagedQuery
{    
    public sealed record GetCommentsCursorQuery
        : IRequest<Result<CursorPagedResult<CommentResponseDTO>>>,
          //ICursorPaginationRequest,
          ICachableRequest
    {
        public Guid ArticleId { get; init; }

        public CursorPaginationParameters PaginationParameters { get; init; } = new()
        {
            Cursor = null,
            PageSize = PaginationConstants.CommentsDefaultPageSize,
            Direction = PaginationDirection.Forward
        };

        //public string? Cursor => null;

        //public int PageSize => PaginationConstants.CommentsDefaultPageSize;

        //public PaginationDirection Direction => PaginationDirection.Forward;

        public IEnumerable<string>? CacheTags => [Common.Caching.CacheTags.Comments];       

        public string GetCacheKeyMetadata() =>
            $"article:{ArticleId}:cursor:{PaginationParameters.Cursor}:size:{PaginationParameters.PageSize}:dir:{PaginationParameters.Direction}";
    }
}
