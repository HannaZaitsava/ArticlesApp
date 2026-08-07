using Application.Abstractions.DataAccess;
using Application.Common.Caching;
using Application.Common.Events;
using Domain.Entities;
using Domain.Errors;
using Domain.Result;
using MediatR;

namespace Application.CQRS.Commands.ArticleCommands.DeleteArticle
{
    internal class DeleteArticleCommandHandler(
        IBaseRepository<Article> repository, 
        IMediator mediator) 
        : IRequestHandler<DeleteArticleCommand, Result<bool>>
    {
        public async Task<Result<bool>> Handle(DeleteArticleCommand request, CancellationToken cancellationToken)
        {
            var articleId = request.Id;

            // для Soft Delete (для заполнения IsDeleted = true) сущность обязательно нужно подгрузить в память           
            var articleEntity = await repository.GetByIdAsync(articleId, true, cancellationToken);

            if (articleEntity is null)
            {
                return Result<bool>.Failure([ArticleErrors.ArticleNotFound(articleId)]);
            }

            // Cache tags to invalidate
            var categoryIds = articleEntity.Categories.Select(c => c.Id).ToList();
            var tagIds = articleEntity.Tags.Select(t => t.Id).ToList();

            repository.Remove(articleEntity);
            await repository.SaveChangesAsync(cancellationToken);

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
