namespace NFT.Contract.Query
{
    public interface INFTQuery
    {
        Task<QueryResult<string>> ENSReverseLookup(string address);
        Task<QueryResult<TransactionState>> GetTransactionStatus(string hash);
        Task<QueryResult<int>> GetNFTCount(string address);
        Task<QueryResult<int>> GetTotalSupply();
        Task<QueryResult<ContractState>> GetContractState();
    }
}
