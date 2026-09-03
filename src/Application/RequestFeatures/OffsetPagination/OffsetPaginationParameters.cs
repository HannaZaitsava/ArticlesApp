using ArticlesApp.Application.Common.Constants;

namespace ArticlesApp.Application.RequestFeatures.OffsetPagination
{
    public sealed record OffsetPaginationParameters
    {
        public int PageIndex { get; set; } = PaginationConstants.MinPageIndex;
        public int PageSize { get; set; } = PaginationConstants.DefaultPageSize;       
    }
}
