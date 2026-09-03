using ArticlesApp.Application.Common.Events;
using ArticlesApp.Application.Abstractions;
using ArticlesApp.Application.Abstractions.DataAccess;
using ArticlesApp.Application.Common.Caching;
using ArticlesApp.Application.DTOs.Articles;
using ArticlesApp.Domain.Entities;
using ArticlesApp.Domain.Errors;
using ArticlesApp.Domain.Result;
using MapsterMapper;
using MediatR;

namespace ArticlesApp.Application.CQRS.Commands.ArticleCommands.CreateArticle
{  
    internal class CreateArticleCommandHandler(
        IBaseRepository<Article> articleRepository,
        IBaseRepository<ArticleCategory> articleCategoryRepository,
        IBaseRepository<Tag> tagRepository,
        IUserContext userContext,
        IMapper mapper,
        IMediator mediator)
        : IRequestHandler<CreateArticleCommand, Result<ArticleResponseDTO>>
    {
        public async Task<Result<ArticleResponseDTO>> Handle(CreateArticleCommand request, CancellationToken cancellationToken)
        {            
            var exists = await articleRepository.IsExistingAsync(a => a.Title == request.Title, cancellationToken);
            
            if (exists)
            {
                return Result<ArticleResponseDTO>.Failure([ArticleErrors.ArticleAlreadyExists(request.Title)]);
            }

            // Маппим основные поля (Title, Content)
            var article = mapper.Map<Article>(request);
            
            // Загружаем связанные сущности из БД по списку ID
            if (request.Tags is { Count: > 0 })
            {
                var foundTags = await tagRepository.GetAllAsync(t => request.Tags.Contains(t.Id), true, cancellationToken);

                var invalidIds = request.Tags.Except(foundTags.Select(t => t.Id));

                if (invalidIds.Any())
                { 
                    return Result<ArticleResponseDTO>.Failure([TagErrors.TagsNotFound(invalidIds)]);
                }
                              
                article.Tags = (ICollection<Tag>)foundTags;
            }

            if (request.Categories is { Count: > 0 })
            {                
                var foundCategories = await articleCategoryRepository.GetAllAsync(t => request.Categories.Contains(t.Id), true, cancellationToken);

                var invalidIds = request.Categories.Except(foundCategories.Select(t => t.Id));

                if (invalidIds.Any())
                {
                    return Result<ArticleResponseDTO>.Failure([ArticleCategoryErrors.ArticleCategoriesNotFound(invalidIds)]);
                }

                article.Categories = (ICollection<ArticleCategory>)foundCategories;
            }
         
            await articleRepository.AddAsync(article, cancellationToken);
            await articleRepository.SaveChangesAsync(cancellationToken);

            // Cache tags to invalidate
            var tagsToInvalidate = new HashSet<string> { CacheTags.Articles };            

            if (request.Categories is not null)
            {
                foreach (var сategory in request.Categories)
                    tagsToInvalidate.Add(CacheTags.ArticleCategory(сategory));
            }

            if (request.Tags is not null)
            {
                foreach (var tag in request.Tags)
                    tagsToInvalidate.Add(CacheTags.Tag(tag));
            }           

            await mediator.Publish(new CacheInvalidationEvent(tagsToInvalidate), cancellationToken);

            var articleResponseDTO = mapper.Map<ArticleResponseDTO>((Article: article, CreatorName: userContext.UserName));

            return Result<ArticleResponseDTO>.Success(articleResponseDTO);
        }
    }
}
