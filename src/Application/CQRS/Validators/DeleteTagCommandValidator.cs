using ArticlesApp.Application.CQRS.Commands.TagCommands.DeleteTag;
using FluentValidation;

namespace ArticlesApp.Application.CQRS.Validators
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
