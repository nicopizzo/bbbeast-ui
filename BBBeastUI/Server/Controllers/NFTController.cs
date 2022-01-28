using BBBeastUI.Models;
using Microsoft.AspNetCore.Mvc;
using NFT.Contract.Query;
using System.Reflection;

namespace BBBeast.UI.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NFTController : ControllerBase
    {
        private readonly INFTQuery _NFTQuery;

        public NFTController(INFTQuery nFTQuery)
        {
            _NFTQuery = nFTQuery;
        }

        [HttpGet("query/count/{address}")]
        public async Task<IActionResult> GetWalletCount(string address)
        {
            try
            {
                return Ok(await _NFTQuery.GetNFTCount(address));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }           
        }

        [HttpGet("query/minted")]
        public async Task<IActionResult> GetTotalMinted()
        {
            return Ok(await _NFTQuery.GetTotalSupply());
        }

        [HttpGet("hash")]
        public async Task<IActionResult> GetProvHashInfo()
        {
            HashDto dto = new HashDto();
            var basePath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            dto.Hashes = await System.IO.File.ReadAllLinesAsync(Path.Combine(basePath, "Hashes", "Hashes.txt"));
            dto.ProvHash = await System.IO.File.ReadAllTextAsync(Path.Combine(basePath, "Hashes", "ProvHash.txt"));
            return Ok(dto);
        }
    }
}
