using Application.Abstractions.DataAccess;
using Application.Common.Caching;
using Application.Common.Events;
using Domain.Entities;
using Domain.Errors;
using Domain.Result;
using MediatR;

namespace Application.CQRS.Commands.CommentCommands.DeleteComment
{
    internal class DeleteCommentCommandHandler(
        IBaseRepository<Comment> repository,
        IMediator mediator)
        : IRequestHandler<DeleteCommentCommand, Result<bool>>
    {
        public async Task<Result<bool>> Handle(DeleteCommentCommand request, CancellationToken cancellationToken)
        {
            var commentId = request.Id;

            var commentEntity = await repository.GetByIdAsync(commentId, trackChanges: true, cancellationToken);

            if (commentEntity is null)
            {
                return Result<bool>.Failure([CommentErrors.CommentNotFound(commentId)]);
            }

            repository.Remove(commentEntity);
            await repository.SaveChangesAsync(cancellationToken);

            // Cache tags to invalidate
            var tagsToInvalidate = new HashSet<string>
            {
                CacheTags.Comment(request.Id)
            };
            if (commentEntity.ParentId is not null)
                tagsToInvalidate.Add(CacheTags.Comment((Guid)commentEntity.ParentId));

            await mediator.Publish(new CacheInvalidationEvent(tagsToInvalidate), cancellationToken);

            return Result<bool>.Success(true);
        }
    }
}
