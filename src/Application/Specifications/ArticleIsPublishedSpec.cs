using ArticlesApp.Application.Abstractions.DataAccess;
using ArticlesApp.Domain.Entities;

namespace ArticlesApp.Application.Specifications
{
    public class ArticleIsPublishedSpec : BaseSpecification<Article>
    {    
        public ArticleIsPublishedSpec(Guid id) : base(a => a.Id == id && a.IsPublished)
        {
        }        
    }
}
