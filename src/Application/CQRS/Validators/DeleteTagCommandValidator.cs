using Application.CQRS.Commands.TagCommands.DeleteTag;
using FluentValidation;

namespace Application.CQRS.Validators
{
    public sealed class DeleteTagCommandValidator : AbstractValidator<DeleteTagCommand>
    {
        public DeleteTagCommandValidator()
        {
            RuleFor(x => x.Id)
                .NotEmpty()
                .WithMessage("Tag ID is required.");
        }
    }
}
