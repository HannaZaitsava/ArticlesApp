using Application.Abstractions.DataAccess;
using Application.Common.Caching;
using Application.Common.Events;
using Application.DTOs.ArticleCategories;
using Domain.Entities;
using Domain.Errors;
using Domain.Result;
using MapsterMapper;
using MediatR;

namespace Application.CQRS.Commands.ArticleCategoryCommands.CreateArticleCategory
{
    internal class CreateArticleCategoryCommandHandler(
        IBaseRepository<ArticleCategory> repository, 
        IMediator mediator,
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

            await mediator.Publish(new CacheInvalidationEvent([CacheTags.ArticleCategories]), cancellationToken);

            var responseDto = mapper.Map<ArticleCategoryResponseDTO>(articleCategory);

            return Result<ArticleCategoryResponseDTO>.Success(responseDto);
        }
    }
}
