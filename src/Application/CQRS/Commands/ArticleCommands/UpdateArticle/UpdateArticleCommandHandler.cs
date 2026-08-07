using Application.Abstractions.DataAccess;
using Application.Common.Caching;
using Application.Common.Events;
using Domain.Entities;
using Domain.Errors;
using Domain.Result;
using MapsterMapper;
using MediatR;

namespace Application.CQRS.Commands.ArticleCommands.UpdateArticle
{
    internal class UpdateArticleCommandHandler(
        IArticleRepository articleRepository,
        IBaseRepository<Tag> tagRepository,
        IBaseRepository<ArticleCategory> categoryRepository,
        IMediator mediator,
        IMapper mapper) : IRequestHandler<UpdateArticleCommand, Result<bool>>
    {
        public async Task<Result<bool>> Handle(UpdateArticleCommand request, CancellationToken cancellationToken)
        {
            var articleId = request.Id;
            
            var article = await articleRepository.GetArticleWithFullInfoAsync(articleId, ct: cancellationToken);

            if (article is null)
            {
                return Result<bool>.Failure([ArticleErrors.ArticleNotFound(articleId)]);
            }

            // Cache tags to invalidate
            var tagsToInvalidate = new HashSet<string>
            {
                CacheTags.Articles,            
                CacheTags.Article(request.Id)  
            };

            // 1. Если поле Tags отсутствует в JSON (null), то  НЕ ТРОГАЕМ существующие теги в базе.
            // 2. Если поле Tags пришло как [] (Count == 0), удаляем все старые связи.
            // 3. Пришли новые ID, то синхронизируем их.
            if (request.Tags is not null)
            {
                if (request.Tags.Count == 0)
                {
                    article.Tags.Clear();
                }
                else
                {
                    var validTagsToAdd = await tagRepository.GetAllAsync(t => request.Tags.Contains(t.Id), true, cancellationToken);

                    var invalidIds = request.Tags.Except(validTagsToAdd.Select(t => t.Id));

                    if (invalidIds.Any())
                    {
                        return Result<bool>.Failure([TagErrors.TagsNotFound(invalidIds)]);
                    }

                    var validTagsToAddIds = validTagsToAdd.Select(x => x.Id).ToList();
                    var tagsToRemove = article.Tags.Where(tag => !validTagsToAddIds.Contains(tag.Id)).ToList();
                    foreach (var tag in tagsToRemove)
                    {
                        article.Tags.Remove(tag);
                        tagsToInvalidate.Add(CacheTags.Tag(tag.Id));
                    }

                    var articleCurrentTagsIds = article.Tags.Select(x => x.Id).ToList();
                    var tagsToAdd = validTagsToAdd.Where(tag => !articleCurrentTagsIds.Contains(tag.Id)).ToList();
                    foreach (var tag in tagsToAdd)
                    { 
                        article.Tags.Add(tag);
                        tagsToInvalidate.Add(CacheTags.Tag(tag.Id));
                    }
                }
            }

            if (request.Categories is not null)
            {
                if (request.Categories.Count == 0)
                {
                    article.Categories.Clear();
                }
                else
                {
                    var validCategoriesToAdd = await categoryRepository.GetAllAsync(t => request.Categories.Contains(t.Id), true, cancellationToken);

                    var invalidIds = request.Categories.Except(validCategoriesToAdd.Select(t => t.Id));

                    if (invalidIds.Any())
                    {
                        return Result<bool>.Failure([ArticleCategoryErrors.ArticleCategoriesNotFound(invalidIds)]);
                    }

                    var validCategoriesToAddIds = validCategoriesToAdd.Select(x => x.Id).ToList();
                    var categoriesToRemove = article.Categories.Where(category => !validCategoriesToAddIds.Contains(category.Id)).ToList();
                    foreach (var category in categoriesToRemove)
                    {
                        article.Categories.Remove(category);
                        tagsToInvalidate.Add(CacheTags.ArticleCategory(category.Id));
                    }

                    var articleCurrentCategoriesIds = article.Categories.Select(x => x.Id).ToList();
                    var categoriesToAdd = validCategoriesToAdd.Where(category => !articleCurrentCategoriesIds.Contains(category.Id)).ToList();
                    foreach (var category in categoriesToAdd)
                    {
                        article.Categories.Add(category);
                        tagsToInvalidate.Add(CacheTags.ArticleCategory(category.Id));
                    }
                }
            }

            mapper.Map(request, article);

            await articleRepository.SaveChangesAsync(cancellationToken);

            await mediator.Publish(new CacheInvalidationEvent(tagsToInvalidate), cancellationToken);

            return Result<bool>.Success(true);
        }
    }
}
