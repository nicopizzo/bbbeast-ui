using Nethereum.Contracts.ContractHandlers;
using Nethereum.Web3;
using nft.contract.query.Queries;
namespace nft.contract.query
{
    public class NFTQuery : INFTQuery
    {
        private ContractHandler _ContractHandler;

        public NFTQuery(IWeb3 web3, string contractAddress)
        {
            _ContractHandler = web3.Eth.GetContractHandler(contractAddress);
        }

        public async Task<int> GetNFTCount(string address)
        {
            var balanceOfFunction = new BalanceOfQuery()
            {
                Owner = address
            };

            var result = await _ContractHandler.QueryAsync<BalanceOfQuery, int>(balanceOfFunction);
            return result;
        }
    }
}
