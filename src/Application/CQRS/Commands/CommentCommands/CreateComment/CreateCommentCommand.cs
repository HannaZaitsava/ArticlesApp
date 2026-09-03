using ArticlesApp.Domain.Result;
using MediatR;

namespace ArticlesApp.Application.CQRS.Commands.CommentCommands.CreateComment
{
    public record CreateCommentCommand(
        Guid ArticleId,
        Guid? ParentId,
        string Text
    ) : IRequest<Result<Guid>>; 
}
