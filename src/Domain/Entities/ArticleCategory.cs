using Domain.Entities.Base;

namespace Domain.Entities
{
    public class ArticleCategory: BaseEntity
    {
        public string Name { get; set; } = null!;

        public bool IsDefault { get; set; }

        public ICollection<Article> Articles { get; set; } = [];
    }
}
