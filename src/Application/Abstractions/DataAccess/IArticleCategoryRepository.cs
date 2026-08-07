using Application.RequestFeatures.OffsetPagination;
using Domain.Entities;

namespace Application.Abstractions.DataAccess
{    
    public interface IArticleCategoryRepository : IBaseRepository<ArticleCategory>
    {
        Task<ArticleCategory?> GetArticleCategoryWithFullInfoAsync(Guid id, bool trackChanges = true, CancellationToken ct = default);
        Task<OffsetPagedResult<TDestinationDTO>> GetOffsetPagedListProjectedAsync<TDestinationDTO>(OffsetPaginationParameters paginationParameters, CancellationToken ct = default);
    }
}
