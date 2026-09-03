using ArticlesApp.Domain.Result;
using MediatR;

namespace ArticlesApp.Application.CQRS.Commands.ArticleCommands.PublishArticleCommand
{
    public sealed record PublishArticleCommand(Guid Id) : IRequest<Result<bool>>;
}
