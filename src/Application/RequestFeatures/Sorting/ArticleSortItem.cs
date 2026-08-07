using Application.Abstractions;
using Application.Enums.SortingEnums;

namespace Application.RequestFeatures.Sorting
{
    public class ArticleSortItem : ISortItem<ArticleSortField>
    {
        public ArticleSortField Field { get; set; }
        public bool IsDescending { get; set; } = false;        
    }
}
