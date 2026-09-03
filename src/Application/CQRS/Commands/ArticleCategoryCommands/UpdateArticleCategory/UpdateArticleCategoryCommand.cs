using ArticlesApp.Domain.Result;
using MediatR;

namespace ArticlesApp.Application.CQRS.Commands.ArticleCategoryCommands.UpdateArticleCategory
{   
    public sealed record UpdateArticleCategoryCommand(Guid Id, string Name) : IRequest<Result<bool>>;
}
