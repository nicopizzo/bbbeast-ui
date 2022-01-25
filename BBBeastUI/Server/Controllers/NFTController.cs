using Microsoft.AspNetCore.Mvc;
using NFT.Contract.Encoding;
using NFT.Contract.Query;

namespace BBBeast.UI.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NFTController : ControllerBase
    {
        private readonly INFTQuery _NFTQuery;
        private readonly INFTEncoding _NFTEncoding;

        public NFTController(INFTQuery nFTQuery, INFTEncoding nFTEncoding)
        {
            _NFTQuery = nFTQuery;
            _NFTEncoding = nFTEncoding;
        }

        [HttpGet("encode/public/{count}")]
        public IActionResult EncodePublic(int count)
        {
            return Ok(_NFTEncoding.GetMintFunctionEncoding(count));
        }

        [HttpGet("encode/private/{count}")]
        public IActionResult EncodePrivate(int count)
        {
            return Ok(_NFTEncoding.GetPrivateSaleMintFunctionEncoding(count));
        }

        [HttpGet("query/{address}")]
        public async Task<IActionResult> GetWalletCount(string address)
        {
            return Ok(await _NFTQuery.GetNFTCount(address));
        }
    }
}
