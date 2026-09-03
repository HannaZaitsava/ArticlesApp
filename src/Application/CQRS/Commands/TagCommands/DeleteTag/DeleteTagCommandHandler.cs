using ArticlesApp.Application.Abstractions.DataAccess;
using ArticlesApp.Application.Common.Caching;
using ArticlesApp.Application.Common.Events;
using ArticlesApp.Domain.Errors;
using ArticlesApp.Domain.Result;
using MediatR;

namespace ArticlesApp.Application.CQRS.Commands.TagCommands.DeleteTag
{
    internal class DeleteTagCommandHandler(
        ITagRepository repository,
        IMediator mediator) 
        : IRequestHandler<DeleteTagCommand, Result<bool>>
    {
        public async Task<Result<bool>> Handle(DeleteTagCommand request, CancellationToken cancellationToken)
        {
            var tagId = request.Id;

            var tagEntity = await repository.GetTagWithFullInfoAsync(tagId, true, cancellationToken);

            if (tagEntity is null)
            {
                return Result<bool>.Failure([TagErrors.TagNotFound(tagId)]);
            }

            // Cache tags to invalidate
            var tagsToInvalidate = new HashSet<string>
            {
                CacheTags.Tags,
                CacheTags.Tag(request.Id)
            };            

            if (tagEntity.Articles is not null)
            {
                foreach (var article in tagEntity.Articles)
                    tagsToInvalidate.Add(CacheTags.Article(article.Id));            
            }

            repository.Remove(tagEntity);
            await repository.SaveChangesAsync(cancellationToken);            

            await mediator.Publish(new CacheInvalidationEvent(tagsToInvalidate), cancellationToken);

            return Result<bool>.Success(true);
        }
    }
}
