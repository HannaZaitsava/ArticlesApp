using Domain.Result;
using MediatR;

namespace Application.CQRS.Commands.TagCommands.DeleteTag
{
    public sealed record DeleteTagCommand(Guid Id) : IRequest<Result<bool>>;
}
