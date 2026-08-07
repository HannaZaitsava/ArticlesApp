using Domain.Result;
using MediatR;

namespace Application.CQRS.Commands.CommentCommands.UpdateComment
{
    public sealed record UpdateCommentCommand(Guid Id, string Text) : IRequest<Result<bool>>;
}
