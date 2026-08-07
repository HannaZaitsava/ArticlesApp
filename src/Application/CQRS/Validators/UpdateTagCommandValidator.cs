using Application.CQRS.Commands.TagCommands.UpdateTag;
using Domain.Constants.EntityConstraints;
using FluentValidation;

namespace Application.CQRS.Validators
{
    public sealed class UpdateTagCommandValidator : AbstractValidator<UpdateTagCommand>
    {
        public UpdateTagCommandValidator()
        {
            RuleFor(x => x.Id)
             .NotEmpty()
             .WithMessage("Tag ID is required.");

            RuleFor(x => x.Label)
               .NotEmpty()
                   .WithMessage("Label is required")
               .Length(TagConstraints.MinLabelLength, TagConstraints.MaxLabelLength)
                   .WithMessage("Label must be between {MinLength} and {MaxLength} characters long");

            RuleFor(v => v.Color)
                // Допускаем null (так как string?). Но если не null, то проверяем формат
                .Matches(TagConstraints.HexColorRegex)
                    .WithMessage("Color must be a valid HEX string (e.g., #FFFFFF)")
                .When(v => v.Color != null);
        }
    }
}
