using ArticlesApp.Domain.Result;
using MediatR;

namespace ArticlesApp.Application.CQRS.Commands.TagCommands.UpdateTag
{
    public sealed record UpdateTagCommand(Guid Id, string? Color, string? Label) : IRequest<Result<bool>>;
}
