using System.Linq.Expressions;

namespace ArticlesApp.Application.Abstractions.DataAccess
{
    public interface ISpecification<TEntity>
    {   
        Expression<Func<TEntity, bool>>? Criteria { get; }                      
    }
}
