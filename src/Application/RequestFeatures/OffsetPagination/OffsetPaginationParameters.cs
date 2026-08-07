using Application.Common.Constants;

namespace Application.RequestFeatures.OffsetPagination
{
    public sealed record OffsetPaginationParameters
    {
        public int PageIndex { get; set; } = PaginationConstants.MinPageIndex;
        public int PageSize { get; set; } = PaginationConstants.DefaultPageSize;       
    }
}
