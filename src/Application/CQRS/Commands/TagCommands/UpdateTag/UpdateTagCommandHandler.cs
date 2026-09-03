using ArticlesApp.Application.Common.Events;
using ArticlesApp.Application.Abstractions.DataAccess;
using ArticlesApp.Application.Common.Caching;
using ArticlesApp.Domain.Errors;
using ArticlesApp.Domain.Result;
using MapsterMapper;
using MediatR;

namespace ArticlesApp.Application.CQRS.Commands.TagCommands.UpdateTag
{
    internal class UpdateTagCommandHandler(
        ITagRepository repository, 
        IMediator mediator,
        IMapper mapper) 
        : IRequestHandler<UpdateTagCommand, Result<bool>>
    {
        public async Task<Result<bool>> Handle(UpdateTagCommand request, CancellationToken cancellationToken)
        {
            var tagId = request.Id;

            var tagEntity = await repository.GetTagWithFullInfoAsync(tagId, true, cancellationToken);

            if (tagEntity is null)
            {
                return Result<bool>.Failure([TagErrors.TagNotFound(tagId)]);
            }

            mapper.Map(request, tagEntity);
            
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
            
            await repository.SaveChangesAsync(cancellationToken);            

            await mediator.Publish(new CacheInvalidationEvent(tagsToInvalidate), cancellationToken);

            return Result<bool>.Success(true);
        }
    }
}
