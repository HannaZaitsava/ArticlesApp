using Application.Abstractions.DataAccess;
using Application.DTOs.Tags;
using Domain.Entities;
using Domain.Errors;
using Domain.Result;
using MediatR;

namespace Application.CQRS.Queries.TagQueries.GetTag
{
    internal class GetTagByIdQueryHandler(IBaseRepository<Tag> repository) : IRequestHandler<GetTagQuery, Result<TagResponseDTO>>
    {
        public async Task<Result<TagResponseDTO>> Handle(GetTagQuery request, CancellationToken cancellationToken)
        {
            Guid tagId = request.Id;

            var tag = await repository.GetByIdProjectedAsync<TagResponseDTO>(tagId, cancellationToken);

            if (tag is null)
            {
                return Result<TagResponseDTO>.Failure([TagErrors.TagNotFound(tagId)]);
            }

            return Result<TagResponseDTO>.Success(tag); 
        }
    }
}
