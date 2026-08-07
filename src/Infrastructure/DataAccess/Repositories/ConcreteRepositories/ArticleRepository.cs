using System.Linq.Expressions;
using Application.Abstractions.DataAccess;
using Application.DTOs.Comments;
using Application.Enums.SortingEnums;
using Application.RequestFeatures.CursorPagination;
using Application.RequestFeatures.OffsetPagination;
using Application.RequestFeatures.Sorting;
using ArticlesApp.Infrastructure.DataAccess.DbContext;
using ArticlesApp.Infrastructure.DataAccess.Extensions;
using ArticlesApp.Infrastructure.DataAccess.Pagination;
using Domain.Entities;
using Mapster;
using MapsterMapper;
using Microsoft.EntityFrameworkCore;

namespace ArticlesApp.Infrastructure.DataAccess.Repositories.ConcreteRepositories
{
    public sealed class ArticleRepository : BaseRepository<Article>, IArticleRepository
    {
        public ArticleRepository(AppDbContext context, IMapper mapper) : base(context, mapper)
        {
        }

        public async Task<Article?> GetArticleWithFullInfoAsync(Guid id, bool trackChanges = true, CancellationToken cancellationToken = default)
        {
            return await _dbSet
                .TrackChanges(trackChanges)
                .Include(a => a.Tags)
                .Include(a => a.Categories)
                .Include(a => a.Comments)
                .AsSplitQuery() 
                .FirstOrDefaultAsync(a => a.Id == id, cancellationToken);
        }

        public async Task<OffsetPagedResult<ArticleShortInfoResponseDTO>> GetArticlesOffsetPagedListProjectedAsync<ArticleShortInfoResponseDTO>(           
            OffsetPaginationParameters paginationParameters,
            ArticleSortItem? sort = null,
            Expression<Func<Article, bool>>? filterPredicate = null,
            CancellationToken ct = default)
        {
            /*
             Для эффективной сортировки нужно, чтобы направление сортировки Id всегда повторяло направление основного поля.

             Если направления полей стали противоположными (DESC и ASC), то
             СУБД не сможет прочитать индекс (CreatedOn ASC, Id ASC) за один проход, так как ей нужно одновременно идти по датам назад, 
             а по Id внутри этих дат — вперед. СУБД будет вынуждена делать операцию сортировки в памяти (Sort Merge / Top N Sort), 
             что на больших объемах данных убьет производительность пагинации.
             */
            bool isDescendingOrder = sort?.IsDescending ?? false;

            (LambdaExpression KeySelector, bool IsDescending)[] sortOrders = [
                ..sort.ToSortingExpressions<Article, ArticleSortField>(),
                ((LambdaExpression)((Article a) => a.Id), isDescendingOrder)
            ];

            return await _dbSet
                   .AsNoTracking()
                   .ApplySortOrders(sortOrders)
                   .FilterByExpression(filterPredicate)
                   .ToOffsetPagedListProjectedAsync<Article, ArticleShortInfoResponseDTO>(paginationParameters, _mapper, ct);
        }
      
        public async Task<CursorPagedResult<CommentResponseDTO>> GetArticleCommentsCursorPaginatedProjectedAsync(
            Guid articleId, 
            CursorPaginationParameters paginationParameters,
            Expression<Func<Comment, bool>>? filterPredicate = null,
            CancellationToken ct = default)
        {     
            return await _context.Comments
                .AsNoTracking()
                .IgnoreQueryFilters() // чтобы сохранить хронологию soft delete, если она используется
                .Where(c => c.ArticleId == articleId)
                .FilterByExpression(filterPredicate)             
                .ApplyCursorPagination(paginationParameters)                
                .ProjectToType<CommentResponseDTO>(_mapper.Config)
                .ToCursorPagedResultAsync(
                    paginationParameters,
                    commentResponseDTO => new CreatedOnCursor(commentResponseDTO.CreatedOn, commentResponseDTO.Id));                 
        }      

        public async Task<OffsetPagedResult<TDestinationDTO>> GetArticleCommentsOffsetPaginatedProjectedAsync<TDestinationDTO>(
           Guid articleId,
           OffsetPaginationParameters paginationParameters,
           //CommentSortItem? sort = null,
           Expression<Func<Comment, bool>>? filterPredicate = null,
           CancellationToken ct = default)
        {
            return await _context.Comments
                   .AsNoTracking() 
                   .IgnoreQueryFilters()
                   .OrderBy(comment => comment.CreatedOn).ThenBy(c => c.Id)
                   .Where(c => c.ArticleId == articleId)
                   .FilterByExpression(filterPredicate)
                   .ToOffsetPagedListProjectedAsync<Comment, TDestinationDTO>(paginationParameters, _mapper, ct);
        }      
    }
}
