using Application.Abstractions.DataAccess;
using Application.Common.Caching;
using Application.Common.Events;
using Domain.Errors;
using Domain.Result;
using MapsterMapper;
using MediatR;

namespace Application.CQRS.Commands.TagCommands.UpdateTag
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

            await repository.SaveChangesAsync(cancellationToken);

            // Cache tags to invalidate
            var tagsToInvalidate = new HashSet<string>
            {
                CacheTags.Tags,
                CacheTags.Tag(request.Id)
            };

            var articleIds = tagEntity.Articles.Select(x => x.Id).ToList();
            foreach (var articleId in articleIds)
                tagsToInvalidate.Add(CacheTags.Article(articleId));

            await mediator.Publish(new CacheInvalidationEvent(tagsToInvalidate), cancellationToken);

            return Result<bool>.Success(true);
        }
    }
}
