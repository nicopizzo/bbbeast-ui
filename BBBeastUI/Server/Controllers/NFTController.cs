using Microsoft.AspNetCore.Mvc;
using NFT.Contract.Query;

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
    }
}
