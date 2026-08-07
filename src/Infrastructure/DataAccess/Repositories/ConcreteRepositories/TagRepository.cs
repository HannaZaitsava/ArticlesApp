using Application.Abstractions.DataAccess;
using Application.RequestFeatures.OffsetPagination;
using ArticlesApp.Infrastructure.DataAccess.DbContext;
using ArticlesApp.Infrastructure.DataAccess.Extensions;
using Domain.Entities;
using MapsterMapper;
using Microsoft.EntityFrameworkCore;

namespace ArticlesApp.Infrastructure.DataAccess.Repositories.ConcreteRepositories
{
    public sealed class TagRepository : BaseRepository<Tag>, ITagRepository
    {
        public TagRepository(AppDbContext context, IMapper mapper) : base(context, mapper)
        {
        }

        public async Task<Tag?> GetTagWithFullInfoAsync(Guid id, bool trackChanges = true, CancellationToken ct = default)
        {
            return await _dbSet
                   .TrackChanges(trackChanges)
                   .Include(a => a.Articles)
                   .FirstOrDefaultAsync(a => a.Id == id, ct);
        }       

        public async Task<OffsetPagedResult<TDestinationDTO>> GetOffsetPagedListProjectedAsync<TDestinationDTO>(
            OffsetPaginationParameters paginationParameters, 
            CancellationToken ct = default)
        {
            return await _dbSet
                   .AsNoTracking()
                   .OrderBy(tag => tag.Label)
                   .ToOffsetPagedListProjectedAsync<Tag, TDestinationDTO>(paginationParameters, _mapper, ct);
        }
    }
}
