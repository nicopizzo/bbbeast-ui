namespace NFT.Contract.Query
{
    public interface INFTQuery
    {
        Task<QueryResult<int>> GetNFTCount(string address);
        Task<QueryResult<int>> GetTotalSupply();
        Task<QueryResult<ContractState>> GetContractState();
    }
}
