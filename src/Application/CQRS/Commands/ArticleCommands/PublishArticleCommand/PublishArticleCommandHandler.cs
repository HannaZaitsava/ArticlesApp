using ArticlesApp.Application.Abstractions.DataAccess;
using ArticlesApp.Application.Common.Caching;
using ArticlesApp.Application.Common.Events;
using ArticlesApp.Domain.Errors;
using ArticlesApp.Domain.Result;
using MediatR;

namespace ArticlesApp.Application.CQRS.Commands.ArticleCommands.PublishArticleCommand
{
    internal class PublishArticleHandler(
        IArticleRepository articleRepository, 
        TimeProvider timeProvider,
        IMediator mediator) 
        : IRequestHandler<PublishArticleCommand, Result<bool>>
    {
        public async Task<Result<bool>> Handle(PublishArticleCommand request, CancellationToken cancellationToken)
        {
            var articleId = request.Id;

            // комментарии подгружать не нужно (их много => убьет производительность)
            var articleEntity = await articleRepository.GetArticleInfoWihoutCommentsAsync(articleId, true, cancellationToken);

            if (articleEntity is null)
            {
                return Result<bool>.Failure([ArticleErrors.ArticleNotFound(articleId)]);
            }
            
            var publishingError = articleEntity.Publish(timeProvider.GetUtcNow());

            if(publishingError is not null)
                return Result<bool>.Failure([publishingError]);

            await articleRepository.SaveChangesAsync(cancellationToken);

            // Cache tags to invalidate 
            var tagsToInvalidate = new HashSet<string>
            {
                CacheTags.Articles,
                CacheTags.Article(articleId),
                CacheTags.ArticleComments(articleId)
            };

            if (articleEntity.Categories is not null)
            {
                foreach (var сategory in articleEntity.Categories)
                    tagsToInvalidate.Add(CacheTags.ArticleCategory(сategory.Id));
            }

            if (articleEntity.Tags is not null)
            {
                foreach (var tag in articleEntity.Tags)
                    tagsToInvalidate.Add(CacheTags.Tag(tag.Id));
            }

            await mediator.Publish(new CacheInvalidationEvent(tagsToInvalidate), cancellationToken);

            return Result<bool>.Success(true);           
        }
    }
}
