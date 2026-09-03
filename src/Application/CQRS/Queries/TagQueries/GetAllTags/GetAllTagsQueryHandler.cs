using ArticlesApp.Application.Abstractions.DataAccess;
using ArticlesApp.Application.DTOs.Tags;
using ArticlesApp.Application.RequestFeatures.OffsetPagination;
using ArticlesApp.Domain.Result;
using MediatR;

namespace ArticlesApp.Application.CQRS.Queries.TagQueries.GetAllTags
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
