using ArticlesApp.Domain.Result;
using MediatR;

namespace ArticlesApp.Application.CQRS.Commands.TagCommands.DeleteTag
{
    public sealed record DeleteTagCommand(Guid Id) : IRequest<Result<bool>>;
}
