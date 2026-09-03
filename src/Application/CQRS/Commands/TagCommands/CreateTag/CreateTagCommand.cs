using ArticlesApp.Application.DTOs.Tags;
using ArticlesApp.Domain.Result;
using MediatR;

namespace ArticlesApp.Application.CQRS.Commands.TagCommands.CreateTag
{
    public record CreateTagCommand(string Label, string? Color) : IRequest<Result<TagResponseDTO>>;
}
