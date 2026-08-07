using Domain.Result;
using MediatR;

namespace Application.CQRS.Commands.CommentCommands.DeleteComment
{    
    public sealed record DeleteCommentCommand(Guid Id) : IRequest<Result<bool>>;
}
