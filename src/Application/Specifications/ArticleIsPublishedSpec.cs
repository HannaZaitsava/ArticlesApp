using Application.Abstractions.DataAccess;
using Domain.Entities;

namespace Application.Specifications
{
    public class ArticleIsPublishedSpec : BaseSpecification<Article>
    {    
        public ArticleIsPublishedSpec(Guid id) : base(a => a.Id == id && a.IsPublished)
        {
        }        
    }
}
