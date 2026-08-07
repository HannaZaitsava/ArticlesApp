using Domain.Result;
using MediatR;

namespace Application.CQRS.Commands.ArticleCommands.PublishArticleCommand
{
    public sealed record PublishArticleCommand(Guid Id) : IRequest<Result<bool>>;
}
