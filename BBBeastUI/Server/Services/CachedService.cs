using Microsoft.Extensions.Caching.Memory;

namespace BBBeast.UI.Server.Services
{
    public abstract class CachedService
    {
        protected readonly IMemoryCache _Cache;

        public CachedService(IMemoryCache cache)
        {
            _Cache = cache;
        }

        protected async Task<TResult> GetCachedValue<TResult>(string key, Func<Task<TResult>> notInCacheFunction, TimeSpan expiration)
        {
            TResult value;
            if (!_Cache.TryGetValue(key, out value))
            {
                value = await notInCacheFunction.Invoke();
                var options = new MemoryCacheEntryOptions()
                {
                    AbsoluteExpirationRelativeToNow = expiration
                };
                _Cache.Set(key, value, options);
            }
            return value;
        }
    }
}
