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
        ICacheInvalidationContext cacheContext)
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
            cacheContext.AddTags([
                CacheTags.Articles,
                CacheTags.Article(articleId),
                CacheTags.ArticleComments(articleId)
                ]);

            if (articleEntity.Categories is not null)
            {
                foreach (var сategory in articleEntity.Categories)
                    cacheContext.AddTag(CacheTags.ArticleCategory(сategory.Id));
            }

            if (articleEntity.Tags is not null)
            {
                foreach (var tag in articleEntity.Tags)
                    cacheContext.AddTag(CacheTags.Tag(tag.Id));
            }

            repository.Remove(articleEntity);
            await repository.SaveChangesAsync(cancellationToken);

            return Result<bool>.Success(true);
        }
    }
}
