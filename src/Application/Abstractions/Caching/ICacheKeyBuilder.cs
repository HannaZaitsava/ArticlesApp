using Application.Common.Caching;

namespace Application.Abstractions.Caching
{
    public interface ICacheKeyBuilder
    {
        string Build(string contextName, string metadata);
    }
}
