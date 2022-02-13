using BBBeast.UI.Shared.Interfaces;
using BBBeast.UI.Shared.Models;
using Microsoft.Extensions.Caching.Memory;
using System.Reflection;

namespace BBBeast.UI.Server.Services
{
    public class ProvHashService : CachedService, IProvenanceHashService
    {
        public ProvHashService(IMemoryCache cache) : base(cache)
        {
        }

        public async Task<HashDto> GetProvenanceHash()
        {
            Func<Task<HashDto>> func = async () =>
            {
                HashDto dto = new();
                var basePath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) ?? "";
                dto.Hashes = await File.ReadAllLinesAsync(Path.Combine(basePath, "Hashes", "Hashes.txt"));
                dto.ProvHash = await File.ReadAllTextAsync(Path.Combine(basePath, "Hashes", "ProvHash.txt"));
                return dto;
            };
            HashDto dto = await GetCachedValue("hashInfo", func, TimeSpan.FromMinutes(30));
            return dto;
        }
    }
}
