using Application.Abstractions.DataAccess;
using Application.Common.Caching;
using Application.Common.Events;
using Domain.Entities;
using Domain.Errors;
using Domain.Result;
using MapsterMapper;
using MediatR;

namespace Application.CQRS.Commands.CommentCommands.UpdateComment
{
    internal class UpdateCommentCommandHandler(
        IBaseRepository<Comment> repository,
        IMediator mediator,
        IMapper mapper)
        : IRequestHandler<UpdateCommentCommand, Result<bool>>
    {
        public async Task<Result<bool>> Handle(UpdateCommentCommand request, CancellationToken cancellationToken)
        {
            var commentId = request.Id;

            var commentEntity = await repository.GetByIdAsync(commentId, true, cancellationToken);

            if (commentEntity is null)
            {
                return Result<bool>.Failure([CommentErrors.CommentNotFound(commentId)]);
            }

            mapper.Map(request, commentEntity);

            await repository.SaveChangesAsync(cancellationToken);

            // Cache Comments to invalidate
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
