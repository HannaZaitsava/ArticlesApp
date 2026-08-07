using Application.CQRS.Commands.CommentCommands.UpdateComment;
using Domain.Constants.EntityConstraints;
using FluentValidation;

namespace Application.CQRS.Validators
{
    public sealed class UpdateCommentCommandValidator : AbstractValidator<UpdateCommentCommand>
    {
        public UpdateCommentCommandValidator()
        {
            RuleFor(x => x.Id)
             .NotEmpty()
             .WithMessage("Comment ID is required.");

            RuleFor(x => x.Text)
               .NotEmpty().WithMessage("Text is required")
               .Length(CommentConstraints.MinTextLength, CommentConstraints.MaxTextLength)
               .WithMessage("Text must be between {MinLength} and {MaxLength} characters");
        }
    }
}
