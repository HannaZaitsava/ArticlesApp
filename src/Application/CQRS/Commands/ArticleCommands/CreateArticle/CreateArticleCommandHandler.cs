using Application.Abstractions;
using Application.Abstractions.DataAccess;
using Application.Common.Caching;
using Application.Common.Events;
using Application.DTOs.Articles;
using Domain.Entities;
using Domain.Errors;
using Domain.Result;
using MapsterMapper;
using MediatR;

namespace Application.CQRS.Commands.ArticleCommands.CreateArticle
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
            //article.PublicationDate = request.PublicationDate ?? DateTimeOffset.UtcNow; - сохранит Auditable

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


            // TODO Позже сделать более гранулированную инвалидацию тегов и Batched-инвалидацию. Перейти на доменные события
            var tagsToInvalidate = new List<string> { CacheTags.Articles };

            if (request.Categories != null)
                tagsToInvalidate.AddRange(request.Categories.Select(CacheTags.ArticleCategory));

            if (request.Tags != null)
                tagsToInvalidate.AddRange(request.Tags.Select(CacheKeys.Tag));

            await mediator.Publish(new CacheInvalidationEvent([CacheTags.Articles]), cancellationToken);

            var articleResponseDTO = mapper.Map<ArticleResponseDTO>((Article: article, CreatorName: userContext.UserName));

            return Result<ArticleResponseDTO>.Success(articleResponseDTO);
        }
    }
}
