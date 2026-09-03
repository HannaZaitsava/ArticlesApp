using ArticlesApp.Domain.Result;
using MediatR;

namespace ArticlesApp.Application.CQRS.Commands.CommentCommands.UpdateComment
{
    public sealed record UpdateCommentCommand(Guid Id, string Text) : IRequest<Result<bool>>;
}
