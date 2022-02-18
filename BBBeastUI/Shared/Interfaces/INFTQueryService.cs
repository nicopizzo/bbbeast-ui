using NFT.Contract.Query;

namespace BBBeast.UI.Shared.Interfaces
{
    public interface INFTQueryService
    {
        Task<QueryResult<string>> ENSReverseLookup(string address, CancellationToken cancellationToken = default);
        Task<QueryResult<int>> GetMintedAmount(string address, CancellationToken cancellationToken = default);
        Task<MintPageResult> GetTotalMinted(CancellationToken cancellationToken = default);
        Task<QueryResult<TransactionState>> GetTransactionState(string tx, CancellationToken cancellationToken = default);
    }
}
