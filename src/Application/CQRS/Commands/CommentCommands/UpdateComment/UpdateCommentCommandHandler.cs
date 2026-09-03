using ArticlesApp.Application.Common.Events;
using ArticlesApp.Application.Abstractions.DataAccess;
using ArticlesApp.Application.Common.Caching;
using ArticlesApp.Domain.Entities;
using ArticlesApp.Domain.Errors;
using ArticlesApp.Domain.Result;
using MapsterMapper;
using MediatR;

namespace ArticlesApp.Application.CQRS.Commands.CommentCommands.UpdateComment
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
                CacheTags.Comment(commentId)
            };
            if (commentEntity.ParentId is not null)
                tagsToInvalidate.Add(CacheTags.Comment((Guid)commentEntity.ParentId));

            /// TODO: возможно, заменить инвалидацию кеша на паттерн Cache Update (Push в кэш): не инвалидировать тег, а асинхронно дописать (делает push) 
            /// комментарий прямо в существующий закэшированный список комментариев в Redis (например, если кэш хранится в виде JSON-массива или структуры данных Redis List).
            await mediator.Publish(new CacheInvalidationEvent(tagsToInvalidate), cancellationToken);

            return Result<bool>.Success(true);
        }
    }
}
