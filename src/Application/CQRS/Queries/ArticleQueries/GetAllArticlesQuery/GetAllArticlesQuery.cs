using ArticlesApp.Application.Common.Caching;
using ArticlesApp.Application.Common.Constants;
using ArticlesApp.Application.DTOs.Articles;
using ArticlesApp.Application.RequestFeatures.OffsetPagination;
using ArticlesApp.Application.RequestFeatures.Sorting;
using ArticlesApp.Domain.Result;
using MediatR;

namespace ArticlesApp.Application.CQRS.Queries.ArticleQueries.GetAllArticlesQuery
{   
    public sealed record GetAllArticlesQuery: 
        IRequest<Result<OffsetPagedResult<ArticleShortInfoResponseDTO>>>, 
        ICachableRequest
    {
        public ArticleSortItem? Sorts { get; init; } = null;
       
        public OffsetPaginationParameters PaginationParameters { get; init; } = new()
        {
            PageIndex = PaginationConstants.MinPageIndex,
            PageSize = PaginationConstants.ArticlesDefaultPageSize,
        };

        public string GetCacheKeyMetadata() =>
            $"p:{PaginationParameters.PageIndex}:s:{PaginationParameters.PageSize}:" +
            //$"q:{Search?.Trim().ToLowerInvariant()}:" +
            $"sort:{Sorts?.Field}:{Sorts?.IsDescending}";

        // для будущего списка сортировок
        // Сортируем параметры сортировки по алфавиту названия поля.
        // Это гарантирует детерминированность ключа, независимо от того, как фронтенд передал массив (в каком поядке пришли параметры сортировки)
        // var sortsJoined = Sorts is not null
        //    ? string.Join(",", Sorts.OrderBy(s => s.Field).Select(s => $"{s.Field}_{s.IsDescending}"))
        //    : "default";
        // Если перейти на IEnumerable<ArticleSortItem>, перед генерацией метаданных обязательно нужно сортировать
        // сам список сортировок(например, по имени поля). Это предотвратит создание разных ключей для Title, Date и Date, Title,
        // если они выдают одинаковый результат в БД.

        public IEnumerable<string>? CacheTags => [Common.Caching.CacheTags.Articles];
    }
}
