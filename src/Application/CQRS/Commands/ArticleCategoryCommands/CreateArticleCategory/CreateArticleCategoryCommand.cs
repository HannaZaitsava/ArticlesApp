using ArticlesApp.Application.DTOs.ArticleCategories;
using ArticlesApp.Domain.Result;
using MediatR;

namespace ArticlesApp.Application.CQRS.Commands.ArticleCategoryCommands.CreateArticleCategory
{
    public sealed record CreateArticleCategoryCommand(string Name) : IRequest<Result<ArticleCategoryResponseDTO>>;
}
