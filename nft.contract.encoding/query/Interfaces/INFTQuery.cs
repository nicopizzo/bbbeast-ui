using NFT.Contract.Models;

namespace NFT.Contract.Query
{
    public interface INFTQuery
    {
        Task<QueryResult> GetNFTCount(string address);
    }
}
