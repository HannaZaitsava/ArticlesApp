using ArticlesApp.Application.CQRS.Commands.CommentCommands.UpdateComment;
using ArticlesApp.Domain.Constants.EntityConstraints;
using FluentValidation;

namespace ArticlesApp.Application.CQRS.Validators
{
    public sealed class UpdateCommentCommandValidator : AbstractValidator<UpdateCommentCommand>
    {
        public UpdateCommentCommandValidator()
        {
            RuleFor(x => x.Id)
                .NotEmpty()
                    .WithMessage("Comment ID is required.");

            RuleFor(x => x.Text)
                .Length(CommentConstraints.MinTextLength, CommentConstraints.MaxTextLength)
                    .WithMessage("Text must be between {MinLength} and {MaxLength} characters")
                    .When(x => x.Text != null); 
        }
    }
}
