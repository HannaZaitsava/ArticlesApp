using ArticlesApp.Application.Abstractions.DataAccess;
using ArticlesApp.Application.Common.Caching;
using ArticlesApp.Application.DTOs.ArticleCategories;
using ArticlesApp.Domain.Entities;
using ArticlesApp.Domain.Errors;
using ArticlesApp.Domain.Result;
using MapsterMapper;
using MediatR;

namespace ArticlesApp.Application.CQRS.Commands.ArticleCategoryCommands.CreateArticleCategory
{
    internal class CreateArticleCategoryCommandHandler(
        IBaseRepository<ArticleCategory> repository,
        ICacheInvalidationContext cacheContext,
        IMapper mapper)
        : IRequestHandler<CreateArticleCategoryCommand, Result<ArticleCategoryResponseDTO>>
    {
        public async Task<Result<ArticleCategoryResponseDTO>> Handle(CreateArticleCategoryCommand request, CancellationToken cancellationToken)
        {
            var exists = await repository.IsExistingAsync(t => t.Name == request.Name, cancellationToken);

            if (exists)
            {
                return Result<ArticleCategoryResponseDTO>.Failure([ArticleCategoryErrors.ArticleCategoryAlreadyExists(request.Name)]);
            }

            var articleCategory = mapper.Map<ArticleCategory>(request);

            await repository.AddAsync(articleCategory, cancellationToken);
            await repository.SaveChangesAsync(cancellationToken);

            cacheContext.AddTag(CacheTags.ArticleCategories);

            var responseDto = mapper.Map<ArticleCategoryResponseDTO>(articleCategory);

            return Result<ArticleCategoryResponseDTO>.Success(responseDto);
        }
    }
}
