using ArticlesApp.Application.Abstractions;
using ArticlesApp.Application.Enums.SortingEnums;

namespace ArticlesApp.Application.RequestFeatures.Sorting
{
    public class ArticleSortItem : ISortItem<ArticleSortField>
    {
        public ArticleSortField Field { get; set; }
        public bool IsDescending { get; set; } = false;        
    }
}
