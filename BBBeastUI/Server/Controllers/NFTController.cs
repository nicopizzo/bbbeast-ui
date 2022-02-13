using BBBeast.UI.Shared.Interfaces;
using Microsoft.AspNetCore.Mvc;
using NFT.Contract.Query;

namespace BBBeast.UI.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NFTController : ControllerBase
    {
        private readonly INFTQueryService _QueryService;
        private readonly IProvenanceHashService _HashService;

        public NFTController(INFTQueryService queryService, IProvenanceHashService hashService)
        {
            _QueryService = queryService;
            _HashService = hashService;
        }

        [HttpGet("query/count/{address}")]
        public async Task<IActionResult> GetWalletCount(string address)
        {
            try
            {
                QueryResult<int> value = await _QueryService.GetMintedAmount(address);
                return Ok(value);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }           
        }

        [HttpGet("query/tx/{hash}")]
        public async Task<IActionResult> GetTransactionStatus(string hash)
        {
            try
            {
                var value = await _QueryService.GetTransactionState(hash);
                return Ok(value);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("query/minted")]
        public async Task<IActionResult> GetMintPageStatus()
        {
            var result = await _QueryService.GetTotalMinted();
            return Ok(result);
        }

        [HttpGet("hash")]
        public async Task<IActionResult> GetProvHashInfo()
        {
            var result = await _HashService.GetProvenanceHash();
            return Ok(result);
        }
    }
}
