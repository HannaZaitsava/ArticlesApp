using Domain.Entities.Base;
using Domain.Errors;
using Domain.Exceptions;

namespace Domain.Entities
{
    public class Article : BaseAuditableEntity
    {
        public string Title { get; set; } = null!;
        public string Content { get; set; } = null!;

        // Храним относительный путь: "/uploads/articles/cover_123.jpg"
        //public string? CoverImagePath { get; set; }

        public ICollection<ArticleCategory> Categories { get; set; } = [];
        public ICollection<Comment> Comments { get; set; } = [];
        public ICollection<Tag> Tags { get; set; } = [];

        public DateTimeOffset? PublicationDate { get; private set; } 
        public bool IsPublished => PublicationDate.HasValue;

        public Error? Publish(DateTimeOffset publishedAt)
        {
            if (IsPublished)
                return ArticleErrors.ArticleAlreadyPublished(Id);

            PublicationDate = publishedAt;

            // //TODO Здесь можно добавить Domain Event, если нужно оповещать другие агрегаты/подсистемы
            // AddDomainEvent(new ArticlePublishedEvent(this));

            return null;
        }
    }
}
