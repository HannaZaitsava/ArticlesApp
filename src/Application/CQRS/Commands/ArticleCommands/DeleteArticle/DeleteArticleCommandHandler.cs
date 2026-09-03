using ArticlesApp.Application.Common.Events;
using ArticlesApp.Application.Abstractions.DataAccess;
using ArticlesApp.Application.Common.Caching;
using ArticlesApp.Domain.Errors;
using ArticlesApp.Domain.Result;
using MediatR;

namespace ArticlesApp.Application.CQRS.Commands.ArticleCommands.DeleteArticle
{
    internal class DeleteArticleCommandHandler(
        IArticleRepository repository, 
        IMediator mediator) 
        : IRequestHandler<DeleteArticleCommand, Result<bool>>
    {
        public async Task<Result<bool>> Handle(DeleteArticleCommand request, CancellationToken cancellationToken)
        {
            var articleId = request.Id;

            var articleEntity = await repository.GetArticleInfoWihoutCommentsAsync(articleId, true, cancellationToken);

            if (articleEntity is null)
            {
                return Result<bool>.Failure([ArticleErrors.ArticleNotFound(articleId)]);
            }

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

            repository.Remove(articleEntity);
            await repository.SaveChangesAsync(cancellationToken);

            await mediator.Publish(new CacheInvalidationEvent(tagsToInvalidate), cancellationToken);

            return Result<bool>.Success(true);
        }
    }
}
