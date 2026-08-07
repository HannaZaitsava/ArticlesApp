using Application.CQRS.Commands.CommentCommands.DeleteComment;
using FluentValidation;

namespace Application.CQRS.Validators
{
    
    public sealed class DeleteCommentCommandValidator : AbstractValidator<DeleteCommentCommand>
    {
        public DeleteCommentCommandValidator()
        {
            RuleFor(x => x.Id)
                .NotEmpty()
                .WithMessage("Comment ID is required.");
        }
    }
}
