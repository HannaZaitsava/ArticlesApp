using Application.Abstractions.DataAccess;
using Application.RequestFeatures.OffsetPagination;
using ArticlesApp.Infrastructure.DataAccess.DbContext;
using ArticlesApp.Infrastructure.DataAccess.Extensions;
using Domain.Entities;
using MapsterMapper;
using Microsoft.EntityFrameworkCore;

namespace ArticlesApp.Infrastructure.DataAccess.Repositories.ConcreteRepositories
{
    public sealed class ArticleCategoryRepository : BaseRepository<ArticleCategory>, IArticleCategoryRepository
    {
        public ArticleCategoryRepository(AppDbContext context, IMapper mapper) : base(context, mapper)
        {
        }

        public async Task<ArticleCategory?> GetArticleCategoryWithFullInfoAsync(Guid id, bool trackChanges, CancellationToken ct)
        {
            return await _dbSet
                    .TrackChanges(trackChanges)
                    .Include(a => a.Articles)
                    .FirstOrDefaultAsync(a => a.Id == id, ct);
        }

        public async Task<OffsetPagedResult<TDestinationDTO>> GetOffsetPagedListProjectedAsync<TDestinationDTO>(OffsetPaginationParameters paginationParameters, CancellationToken ct = default)
        {
            return await _dbSet
                   .AsNoTracking()
                   .OrderBy(categoty => categoty.Name) 
                   .ToOffsetPagedListProjectedAsync<ArticleCategory, TDestinationDTO>(paginationParameters, _mapper, ct);
        }
    }
}
