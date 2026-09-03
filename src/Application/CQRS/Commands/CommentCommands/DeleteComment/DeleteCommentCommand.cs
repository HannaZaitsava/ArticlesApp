using ArticlesApp.Domain.Result;
using MediatR;

namespace ArticlesApp.Application.CQRS.Commands.CommentCommands.DeleteComment
{    
    public sealed record DeleteCommentCommand(Guid Id) : IRequest<Result<bool>>;
}
