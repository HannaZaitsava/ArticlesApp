using System.Linq.Expressions;

namespace Application.Abstractions.DataAccess
{
    public interface ISpecification<TEntity>
    {   
        Expression<Func<TEntity, bool>>? Criteria { get; }                      
    }
}
