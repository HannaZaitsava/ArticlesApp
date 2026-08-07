namespace Application.Abstractions.Caching
{
    public interface ICacheService
    {       
        ValueTask<TResponse> GetOrSetAsync<TResponse>(
           string requestCacheKey,
           Func<CancellationToken, ValueTask<TResponse>> factory,
           int? requestExpirationSeconds,
           int? requestLocalCacheExpirationSeconds,
           bool bypassCacheRead,
           IEnumerable<string>? tags,
           CancellationToken ct = default);

        ValueTask RemoveByTagAsync(string tag, CancellationToken ct = default);
        ValueTask RemoveByTagsAsync(IEnumerable<string> tags, CancellationToken ct = default);
        ValueTask RemoveByKeyAsync(string key, CancellationToken ct = default);
        ValueTask RemoveByKeysAsync(IEnumerable<string> keys, CancellationToken ct = default);
    }
}
