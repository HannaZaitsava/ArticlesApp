using Application.Abstractions.DataAccess;
using Application.DTOs.Tags;
using Application.RequestFeatures.OffsetPagination;
using Domain.Result;
using MediatR;

namespace Application.CQRS.Queries.TagQueries.GetAllTags
{    
    internal class GetAllTagsQueryHandler(
        ITagRepository tagRepository)
        : IRequestHandler<GetAllTagsQuery, Result<OffsetPagedResult<TagShortInfoResponseDTO>>>
    {
        public async Task<Result<OffsetPagedResult<TagShortInfoResponseDTO>>> Handle(GetAllTagsQuery request, CancellationToken cancellationToken)
        {
            var tags = await tagRepository.GetOffsetPagedListProjectedAsync<TagShortInfoResponseDTO>(request.PaginationParameters, cancellationToken);

            return Result<OffsetPagedResult<TagShortInfoResponseDTO>>.Success(tags);
        }
    }
}
