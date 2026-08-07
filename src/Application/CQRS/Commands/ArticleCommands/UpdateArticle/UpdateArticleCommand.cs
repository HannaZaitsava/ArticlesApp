using Domain.Result;
using MediatR;

namespace Application.CQRS.Commands.ArticleCommands.UpdateArticle
{
    public sealed record UpdateArticleCommand() : IRequest<Result<bool>>
    {
        public Guid Id { get; set; }
        public string Title { get; set; } = null!;
        public string Content { get; set; } = null!;

        public IReadOnlyCollection<Guid>? Categories { get; set; }
        public IReadOnlyCollection<Guid>? Tags { get; set; }
    }
}
