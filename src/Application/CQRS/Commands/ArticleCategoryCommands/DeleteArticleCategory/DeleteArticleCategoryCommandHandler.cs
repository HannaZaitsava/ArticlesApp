using ArticlesApp.Application.Abstractions.DataAccess;
using ArticlesApp.Application.Common.Caching;
using ArticlesApp.Application.Common.Events;
using ArticlesApp.Domain.Errors;
using ArticlesApp.Domain.Result;
using MediatR;

namespace ArticlesApp.Application.CQRS.Commands.ArticleCategoryCommands.DeleteArticleCategory
{
    internal class DeleteArticleCategoryCommandHandler(
        IArticleCategoryRepository repository,
        IMediator mediator) 
        : IRequestHandler<DeleteArticleCategoryCommand, Result<bool>>
    {
        public async Task<Result<bool>> Handle(DeleteArticleCategoryCommand request, CancellationToken cancellationToken)
        {
            var articleCategoryId = request.Id;
           
            var articleCategoryEntity = await repository.GetArticleCategoryWithFullInfoAsync(articleCategoryId, true, cancellationToken);

            if (articleCategoryEntity is null)
            {
                return Result<bool>.Failure([ArticleCategoryErrors.ArticleCategoryNotFound(articleCategoryId)]);
            }

            // Cache tags to invalidate
            var tagsToInvalidate = new HashSet<string>
            {
                CacheTags.ArticleCategories,
                CacheTags.ArticleCategory(request.Id)
            };

            if (articleCategoryEntity.Articles is not null)
            {
                foreach (var article in articleCategoryEntity.Articles)
                    tagsToInvalidate.Add(CacheTags.Article(article.Id));
            }           

            repository.Remove(articleCategoryEntity);
            await repository.SaveChangesAsync(cancellationToken);              

            await mediator.Publish(new CacheInvalidationEvent(tagsToInvalidate), cancellationToken);

            return Result<bool>.Success(true);
        }
    }
}
