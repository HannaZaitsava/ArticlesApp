using Domain.Result;
using MediatR;

namespace Application.CQRS.Commands.TagCommands.UpdateTag
{
    public sealed record UpdateTagCommand(Guid Id, string? Color, string Label = default!) : IRequest<Result<bool>>;
}
