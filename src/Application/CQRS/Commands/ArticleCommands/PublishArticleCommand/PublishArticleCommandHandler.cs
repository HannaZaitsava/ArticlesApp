using Application.Abstractions.DataAccess;
using Application.Common.Caching;
using Application.Common.Events;
using Domain.Entities;
using Domain.Errors;
using Domain.Result;
using MediatR;

namespace Application.CQRS.Commands.ArticleCommands.PublishArticleCommand
{
    internal class PublishArticleHandler(
        IBaseRepository<Article> articleRepository, 
        TimeProvider timeProvider,
        IMediator mediator) 
        : IRequestHandler<PublishArticleCommand, Result<bool>>
    {
        public async Task<Result<bool>> Handle(PublishArticleCommand request, CancellationToken cancellationToken)
        {
            var articleId = request.Id;

            var articleEntity = await articleRepository.GetByIdAsync(articleId, true, cancellationToken);

            if (articleEntity is null)
            {
                return Result<bool>.Failure([ArticleErrors.ArticleNotFound(articleId)]);
            }
            
            var publishingError = articleEntity.Publish(timeProvider.GetUtcNow());

            if(publishingError is not null)
                return Result<bool>.Failure([publishingError]);

            await articleRepository.SaveChangesAsync(cancellationToken);

            // Cache tags to invalidate
            var categoryIds = articleEntity.Categories.Select(c => c.Id).ToList();
            var tagIds = articleEntity.Tags.Select(t => t.Id).ToList();

            var tagsToInvalidate = new HashSet<string>
                {
                    CacheTags.Articles,
                    CacheTags.Article(request.Id)
                };

            foreach (var catId in categoryIds)
                tagsToInvalidate.Add(CacheTags.ArticleCategory(catId));

            foreach (var tagId in tagIds)
                tagsToInvalidate.Add(CacheTags.Tag(tagId));

            await mediator.Publish(new CacheInvalidationEvent(tagsToInvalidate), cancellationToken);

            return Result<bool>.Success(true);           
        }
    }
}
