using Application.DTOs.ArticleCategories;
using Domain.Result;
using MediatR;

namespace Application.CQRS.Commands.ArticleCategoryCommands.CreateArticleCategory
{
    public sealed record CreateArticleCategoryCommand(string Name) : IRequest<Result<ArticleCategoryResponseDTO>>;
}
