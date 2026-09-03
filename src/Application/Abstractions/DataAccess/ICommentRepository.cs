using ArticlesApp.Domain.Entities;

namespace ArticlesApp.Application.Abstractions.DataAccess
{
    public interface ICommentRepository : IBaseRepository<Comment>
    {
        Task<IList<TDestinationDTO>> GetNestedCommentsProjectedAsync<TDestinationDTO>(IEnumerable<Guid> rootCommentsIds, CancellationToken ct = default);
    }
}
