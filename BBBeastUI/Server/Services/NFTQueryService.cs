using BBBeast.UI.Shared.Interfaces;
using Microsoft.Extensions.Caching.Memory;
using NFT.Contract.Query;

namespace BBBeast.UI.Server.Services
{
    public class NFTQueryService : CachedService, INFTQueryService
    {
        private readonly INFTQuery _NFTQuery;

        public NFTQueryService(INFTQuery nFTQuery, IMemoryCache cache) : base(cache)
        {
            _NFTQuery = nFTQuery;
        }

        public async Task<QueryResult<string>> ENSReverseLookup(string address, CancellationToken cancellationToken = default)
        {
            Func<Task<QueryResult<string>>> func = async () =>
            {
                return await _NFTQuery.ENSReverseLookup(address);
            };
            QueryResult<string> ens = await GetCachedValue($"ens{address}", func, TimeSpan.FromHours(1));
            return ens;
        }

        public async Task<QueryResult<int>> GetMintedAmount(string address, CancellationToken cancellationToken = default)
        {
            QueryResult<int> value = await _NFTQuery.GetNFTCount(address);
            return value;
        }

        public async Task<MintPageResult> GetTotalMinted(CancellationToken cancellationToken = default)
        {
            Task<QueryResult<int>> supplyTask = _NFTQuery.GetTotalSupply();

            Func<Task<QueryResult<ContractState>>> func = async () =>
            {
                return await _NFTQuery.GetContractState();
            };

            Task<QueryResult<ContractState>> stateTask = GetCachedValue("state", func, TimeSpan.FromMinutes(1));
            await Task.WhenAll(new List<Task>() { supplyTask, stateTask });

            var supply = await supplyTask;
            var state = await stateTask;

            return new MintPageResult() { TotalMinted = supply.Data, ContractState = state.Data };
        }

        public async Task<QueryResult<TransactionState>> GetTransactionState(string tx, CancellationToken cancellationToken = default)
        {
            Func<Task<QueryResult<TransactionState>>> func = async () =>
            {
                return await _NFTQuery.GetTransactionStatus(tx);
            };

            QueryResult<TransactionState> value = await GetCachedValue($"hash{tx}", func, TimeSpan.FromSeconds(10));
            return value;
        }
    }
}
