using ArticlesApp.Application.RequestFeatures.OffsetPagination;
using ArticlesApp.Domain.Entities;

namespace ArticlesApp.Application.Abstractions.DataAccess
{    
    public interface ITagRepository : IBaseRepository<Tag>
    {
        Task<Tag?> GetTagWithFullInfoAsync(Guid id, bool trackChanges = true, CancellationToken ct = default);
        Task<OffsetPagedResult<TDestinationDTO>> GetOffsetPagedListProjectedAsync<TDestinationDTO>(OffsetPaginationParameters paginationParameters, CancellationToken ct = default);
    }
}
