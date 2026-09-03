using ArticlesApp.Application.Common.Constants;
using ArticlesApp.Application.Enums;

namespace ArticlesApp.Application.RequestFeatures.CursorPagination
{
    public record CursorPaginationParameters(
     string? Cursor = null,
     int PageSize = PaginationConstants.DefaultPageSize,
     PaginationDirection Direction = PaginationDirection.Forward)
    {
    }
}
