using Application.Common.Constants;
using Application.Enums;

namespace Application.RequestFeatures.CursorPagination
{
    public record CursorPaginationParameters(
     string? Cursor = null,
     int PageSize = PaginationConstants.DefaultPageSize,
     PaginationDirection Direction = PaginationDirection.Forward)
    {
    }
}
