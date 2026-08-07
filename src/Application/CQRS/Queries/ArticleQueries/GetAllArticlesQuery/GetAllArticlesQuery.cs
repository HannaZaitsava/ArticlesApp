using Application.Common.Caching;
using Application.Common.Constants;
using Application.DTOs.Articles;
using Application.RequestFeatures.OffsetPagination;
using Application.RequestFeatures.Sorting;
using Domain.Result;
using MediatR;

namespace Application.CQRS.Queries.ArticleQueries.GetAllArticlesQuery
{
    //public class GetAllArticlesQuery : IRequest<PagedResult<ArticleResponseDTO>>
    //{
    //    public int PageIndex { get; set; } = 1;
    //    public int PageSize { get; set; } = 2;
    //    public List<SortItem<ArticleSortField>>? Sorts { get; set; }
    //    public List<ArticleSortItem>? Sorts1 { get; set; }
    //}

    //public record GetAllArticlesQuery (int PageIndex, int PageSize, List<SortItem<ArticleSortField>>? Sorts) : IRequest<PagedResult<ArticleResponseDTO>>;

    public sealed record GetAllArticlesQuery: 
        IRequest<Result<OffsetPagedResult<ArticleShortInfoResponseDTO>>>, 
        ICachableRequest
    {
        public ArticleSortItem? Sorts { get; init; } = null;
        //public int PageIndex { get; init; } = PaginationConstants.MinPageIndex;
        //public int PageSize { get; init; } = PaginationConstants.ArticlesDefaultPageSize;

        public OffsetPaginationParameters PaginationParameters { get; init; } = new()
        {
            PageIndex = PaginationConstants.MinPageIndex,
            PageSize = PaginationConstants.ArticlesDefaultPageSize,
        };

        public string GetCacheKeyMetadata() =>
            $"p:{PaginationParameters.PageIndex}:s:{PaginationParameters.PageSize}:" +
            //$"q:{Search?.Trim().ToLowerInvariant()}:" +
            $"sort:{Sorts?.Field}:{Sorts?.IsDescending}";
        // $"sorts:{string.Join(",", Sorts.Select(s => $"{s.Field}_{s.IsDescending}"))}" // для будущего списка сортировок
        // Если перейти на IEnumerable<ArticleSortItem>, перед генерацией метаданных обязательно нужно сортировать
        // сам список сортировок(например, по имени поля). Это предотвратит создание разных ключей для Title, Date и Date, Title,
        // если они выдают одинаковый результат в БД.

        public IEnumerable<string>? CacheTags => [Common.Caching.CacheTags.Articles];
    }
}
