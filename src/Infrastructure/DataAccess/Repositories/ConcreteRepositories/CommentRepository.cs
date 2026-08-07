using Application.Abstractions.DataAccess;
using ArticlesApp.Infrastructure.DataAccess.DbContext;
using Domain.Entities;
using Mapster;
using MapsterMapper;
using Microsoft.EntityFrameworkCore;

namespace ArticlesApp.Infrastructure.DataAccess.Repositories.ConcreteRepositories
{
    public sealed class CommentRepository : BaseRepository<Comment>, ICommentRepository
    {
        public CommentRepository(AppDbContext context, IMapper mapper) : base(context, mapper)
        {
        }       

        public async Task<IList<TDestinationDTO>> GetNestedCommentsProjectedAsync<TDestinationDTO>(
           IEnumerable<Guid> rootCommentsIds,           
           CancellationToken cancellationToken = default)
        {
            return await _dbSet
                .AsNoTracking()
                .IgnoreQueryFilters() // чтобы сохранить хронологию soft delete, если она используется
                .Where(c => c.RootCommentId != null && rootCommentsIds.Contains(c.RootCommentId.Value))
                .ProjectToType<TDestinationDTO>(_mapper.Config)
                .ToListAsync(cancellationToken);
        }      
    }
}
