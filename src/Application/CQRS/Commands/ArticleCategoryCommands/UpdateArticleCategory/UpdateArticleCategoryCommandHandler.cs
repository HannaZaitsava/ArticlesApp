using ArticlesApp.Application.Common.Events;
using ArticlesApp.Application.Abstractions.DataAccess;
using ArticlesApp.Application.Common.Caching;
using ArticlesApp.Domain.Errors;
using ArticlesApp.Domain.Result;
using MapsterMapper;
using MediatR;

namespace ArticlesApp.Application.CQRS.Commands.ArticleCategoryCommands.UpdateArticleCategory
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

            await repository.SaveChangesAsync(cancellationToken);            

            await mediator.Publish(new CacheInvalidationEvent(tagsToInvalidate), cancellationToken);

            return Result<bool>.Success(true);
        }
    }
}
