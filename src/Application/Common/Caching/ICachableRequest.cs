namespace Application.Common.Caching
{
    public interface ICachableRequest
    {
        bool BypassCache => false;
        //string CacheKey { get; }      
        string GetCacheKeyMetadata();
        IEnumerable<string>? CacheTags => [];
        int ExpirationSeconds => 20;
        int LocalCacheExpirationSeconds => 10;
    }
}
