using System.Linq.Expressions;
using ArticlesApp.Application.DTOs.Comments;
using ArticlesApp.Application.RequestFeatures.CursorPagination;
using ArticlesApp.Application.RequestFeatures.OffsetPagination;
using ArticlesApp.Application.RequestFeatures.Sorting;
using ArticlesApp.Domain.Entities;

namespace ArticlesApp.Application.Abstractions.DataAccess
{
    public interface IArticleRepository: IBaseRepository<Article>
    {        
        Task<Article?> GetArticleWithFullInfoAsync(
            Guid id, 
            bool trackChanges = true, 
            CancellationToken ct = default);

        Task<Article?> GetArticleInfoWihoutCommentsAsync(
            Guid id,
            bool trackChanges = true,
            CancellationToken ct = default);

        Task<OffsetPagedResult<TDestinationDTO>> GetArticleCommentsOffsetPaginatedProjectedAsync<TDestinationDTO>(
            Guid articleId, 
            OffsetPaginationParameters paginationParameters, 
            Expression<Func<Comment, bool>>? filterPredicate = null, 
            CancellationToken ct = default);

        Task<CursorPagedResult<CommentResponseDTO>> GetArticleCommentsCursorPaginatedProjectedAsync(
             Guid articleId,
             CursorPaginationParameters paginationParameters,
             Expression<Func<Comment, bool>>? predicate = null,
             CancellationToken ct = default);

        Task<OffsetPagedResult<ArticleShortInfoResponseDTO>> GetArticlesOffsetPagedListProjectedAsync<ArticleShortInfoResponseDTO>(
            OffsetPaginationParameters paginationParameters, 
            ArticleSortItem? sort = null, 
            Expression<Func<Article, bool>>? filterPredicate = null, 
            CancellationToken ct = default);
    }
}
