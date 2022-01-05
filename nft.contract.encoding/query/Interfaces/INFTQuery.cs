using System.Numerics;

namespace nft.contract.query
{
    public interface INFTQuery
    {
        Task<int> GetNFTCount(string address);
    }
}
