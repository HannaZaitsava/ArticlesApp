using Application.RequestFeatures.Sorting;
using ArticlesAPI.Models.Common;

namespace ArticlesAPI.Models.Requests
{
    public sealed record GetArticlesPaginatedApiRequest(
        ArticleSortItem? Sorts
        //List<Sorting>? SortsNEW = null,
        //List<ArticleSortItem>? SortsNEW = null
        ) : OffsetPaginationApiRequest;
}
