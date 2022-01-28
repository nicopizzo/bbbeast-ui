namespace NFT.Contract.Query
{
    public interface INFTQuery
    {
        Task<QueryResult> GetNFTCount(string address);
        Task<QueryResult> GetTotalSupply();
    }
}
