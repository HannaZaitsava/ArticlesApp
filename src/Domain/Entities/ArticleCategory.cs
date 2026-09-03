using ArticlesApp.Domain.Entities.Base;

namespace ArticlesApp.Domain.Entities
{
    public class ArticleCategory: BaseEntity
    {
        public string Name { get; set; } = null!;

        public bool IsDefault { get; set; }

        public ICollection<Article> Articles { get; set; } = [];
    }
}
