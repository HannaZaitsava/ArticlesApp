using ArticlesApp.Application.RequestFeatures.Sorting;
using ArticlesApp.ArticlesAPI.Models.Common;

namespace ArticlesApp.ArticlesAPI.Models.Requests
{
    public sealed record GetArticlesPaginatedApiRequest(
        ArticleSortItem? Sorts
        //List<Sorting>? SortsNEW = null,
        //List<ArticleSortItem>? SortsNEW = null
        ) : OffsetPaginationApiRequest;
}
