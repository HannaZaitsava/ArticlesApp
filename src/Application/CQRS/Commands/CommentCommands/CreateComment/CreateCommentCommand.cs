using Domain.Result;
using MediatR;

namespace Application.CQRS.Commands.CommentCommands.CreateComment
{
    public record CreateCommentCommand(
        Guid ArticleId,
        Guid? ParentId,
        string Text
    ) : IRequest<Result<Guid>>; 
}
