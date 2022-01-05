using System.Numerics;

namespace nft.contract.query
{
    public interface INFTQuery
    {
        Task<BigInteger> GetNFTCount(string address);
    }
}
