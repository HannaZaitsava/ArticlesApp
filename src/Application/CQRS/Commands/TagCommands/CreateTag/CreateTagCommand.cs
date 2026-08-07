using Application.DTOs.Tags;
using Domain.Result;
using MediatR;

namespace Application.CQRS.Commands.TagCommands.CreateTag
{
    public record CreateTagCommand(string Label, string? Color) : IRequest<Result<TagResponseDTO>>;
}
