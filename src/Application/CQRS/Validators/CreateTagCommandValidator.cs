using Application.CQRS.Commands.TagCommands.CreateTag;
using Domain.Constants.EntityConstraints;
using FluentValidation;

namespace Application.CQRS.Validators
{
    public sealed class CreateTagCommandValidator : AbstractValidator<CreateTagCommand>
    {
        public CreateTagCommandValidator()
        {          
            RuleFor(x => x.Label)
            .NotEmpty()
                .WithMessage("Label is required")
            .Length(TagConstraints.MinLabelLength, TagConstraints.MaxLabelLength)
                .WithMessage("Label must be between {MinLength} and {MaxLength} characters long");

            RuleFor(v => v.Color)
                // Допускаем null (так как string?), но если не null, то проверяем формат
                .Matches(TagConstraints.HexColorRegex)
                    .WithMessage("Color must be a valid HEX string (e.g., #FFFFFF)")
                .When(v => v.Color != null);
        }
    }
}
