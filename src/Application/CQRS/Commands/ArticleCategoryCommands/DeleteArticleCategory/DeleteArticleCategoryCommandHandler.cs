using Application.Abstractions.DataAccess;
using Application.Common.Caching;
using Application.Common.Events;
using Domain.Entities;
using Domain.Errors;
using Domain.Result;
using MediatR;

namespace Application.CQRS.Commands.ArticleCategoryCommands.DeleteArticleCategory
{
    internal class DeleteArticleCategoryCommandHandler(
        IBaseRepository<ArticleCategory> repository,
        IMediator mediator) 
        : IRequestHandler<DeleteArticleCategoryCommand, Result<bool>>
    {
        public async Task<Result<bool>> Handle(DeleteArticleCategoryCommand request, CancellationToken cancellationToken)
        {
            var articleCategoryId = request.Id;
           
            var articleCategoryEntity = await repository.GetByIdAsync(articleCategoryId, true, cancellationToken);

            if (articleCategoryEntity is null)
            {
                return Result<bool>.Failure([ArticleCategoryErrors.ArticleCategoryNotFound(articleCategoryId)]);
            }

            repository.Remove(articleCategoryEntity);
            await repository.SaveChangesAsync(cancellationToken);

            // Cache tags to invalidate
            var tagsToInvalidate = new HashSet<string>
            {
                CacheTags.ArticleCategories,
                CacheTags.ArticleCategory(request.Id)
            };

            var articleIds = articleCategoryEntity.Articles.Select(x => x.Id).ToList();
            foreach (var articleId in articleIds)
                tagsToInvalidate.Add(CacheTags.Article(articleId));

            await mediator.Publish(new CacheInvalidationEvent(tagsToInvalidate), cancellationToken);

            return Result<bool>.Success(true);
        }
    }
}
