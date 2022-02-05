using Nethereum.Contracts.ContractHandlers;
using Nethereum.Web3;
using NFT.Contract.Query.Queries;

namespace NFT.Contract.Query
{
    public class NFTQuery : INFTQuery
    {
        private ContractHandler _ContractHandler;

        public NFTQuery(IWeb3 web3, string contractAddress)
        {
            _ContractHandler = web3.Eth.GetContractHandler(contractAddress);
        }

        public async Task<QueryResult<ContractState>> GetContractState()
        {
            QueryResult<ContractState> state = new QueryResult<ContractState>() { Data = ContractState.NotLive };

            var isLiveFunction = new IsLiveQuery();
            var result = await _ContractHandler.QueryAsync<IsLiveQuery, bool>(isLiveFunction);
            if (result)
            {
                state.Data = ContractState.Public;
                return state;
            }

            var isPrivateLiveFunction = new IsPrivateLiveQuery();
            result = await _ContractHandler.QueryAsync<IsPrivateLiveQuery, bool>(isPrivateLiveFunction);
            if (result)
            {
                state.Data = ContractState.Private;
            }

            return state;
        }

        public async Task<QueryResult<int>> GetNFTCount(string address)
        {
            var balanceOfFunction = new BalanceOfQuery()
            {
                Owner = address
            };

            var result = await _ContractHandler.QueryAsync<BalanceOfQuery, int>(balanceOfFunction);
            return new QueryResult<int>() { Data = result };
        }

        public async Task<QueryResult<int>> GetTotalSupply()
        {
            var supplyFunction = new TotalSupplyQuery();

            var result = await _ContractHandler.QueryAsync<TotalSupplyQuery, int>(supplyFunction);
            return new QueryResult<int>() { Data = result };
        }
    }
}
