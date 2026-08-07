using Application.Abstractions.DataAccess;
using Application.Common.Caching;
using Application.Common.Events;
using Domain.Errors;
using Domain.Result;
using MapsterMapper;
using MediatR;

namespace Application.CQRS.Commands.ArticleCategoryCommands.UpdateArticleCategory
{   
     internal class UpdateArticleCategoryCommandHandler(
        IArticleCategoryRepository repository, 
        IMediator mediator,
        IMapper mapper) 
        : IRequestHandler<UpdateArticleCategoryCommand, Result<bool>>
    {
        public async Task<Result<bool>> Handle(UpdateArticleCategoryCommand request, CancellationToken cancellationToken)
        {
            var articleCategoryId = request.Id;

            var articleCategoryEntity = await repository.GetArticleCategoryWithFullInfoAsync(articleCategoryId, ct: cancellationToken);

            if (articleCategoryEntity is null)
            {
                return Result<bool>.Failure([ArticleCategoryErrors.ArticleCategoryNotFound(articleCategoryId)]);
            }

            mapper.Map(request, articleCategoryEntity);

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
