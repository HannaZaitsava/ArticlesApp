using ArticlesApp.Application.DTOs.Articles;

namespace ArticlesApp.Application.DTOs.ArticleCategories
{
    public sealed record ArticleCategoryResponseDTO
    {
        public Guid Id { get; init; }
        public string Name { get; init; } = null!;
        public IReadOnlyCollection<ArticleForTagResponseDTO> Articles { get; init; } = [];
    }
}
