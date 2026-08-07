using Domain.Result;
using MediatR;

namespace Application.CQRS.Commands.ArticleCategoryCommands.DeleteArticleCategory
{   
    public sealed record DeleteArticleCategoryCommand(Guid Id) : IRequest<Result<bool>>;
}
