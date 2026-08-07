using Domain.Entities.Base;

namespace Domain.Entities
{
    public class Tag : BaseEntity
    {      
        public string Label { get; set; } = null!;

        public string? Color { get; set; }

        public ICollection<Article> Articles { get; set; } = [];
    }
}
