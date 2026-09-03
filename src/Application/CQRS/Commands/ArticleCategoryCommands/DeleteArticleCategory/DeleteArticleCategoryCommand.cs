using ArticlesApp.Domain.Result;
using MediatR;

namespace ArticlesApp.Application.CQRS.Commands.ArticleCategoryCommands.DeleteArticleCategory
{   
    public sealed record DeleteArticleCategoryCommand(Guid Id) : IRequest<Result<bool>>;
}
