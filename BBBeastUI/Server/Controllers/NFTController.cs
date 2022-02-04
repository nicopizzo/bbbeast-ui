using BBBeastUI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using NFT.Contract.Query;
using System.Reflection;

namespace BBBeast.UI.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NFTController : ControllerBase
    {
        private readonly INFTQuery _NFTQuery;
        private readonly IMemoryCache _Cache;

        public NFTController(INFTQuery nFTQuery, IMemoryCache cache)
        {
            _NFTQuery = nFTQuery;
            _Cache = cache;
        }

        [HttpGet("query/count/{address}")]
        public async Task<IActionResult> GetWalletCount(string address)
        {
            try
            {
                QueryResult value = await _NFTQuery.GetNFTCount(address);
                return Ok(value);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }           
        }

        [HttpGet("query/minted")]
        public async Task<IActionResult> GetTotalMinted()
        {
            QueryResult value = await _NFTQuery.GetTotalSupply();
            return Ok(value);
        }

        [HttpGet("hash")]
        public async Task<IActionResult> GetProvHashInfo()
        {
            Func<Task<HashDto>> func = async () =>
            {
                HashDto dto = new();
                var basePath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
                dto.Hashes = await System.IO.File.ReadAllLinesAsync(Path.Combine(basePath, "Hashes", "Hashes.txt"));
                dto.ProvHash = await System.IO.File.ReadAllTextAsync(Path.Combine(basePath, "Hashes", "ProvHash.txt"));
                return dto;
            };
            HashDto dto = await GetCachedValue("hashInfo", func, TimeSpan.FromMinutes(30));
            return Ok(dto);
        }

        private async Task<TResult> GetCachedValue<TResult>(string key, Func<Task<TResult>> notInCacheFunction, TimeSpan expiration)
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
