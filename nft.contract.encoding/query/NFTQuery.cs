using Nethereum.Contracts.ContractHandlers;
using Nethereum.JsonRpc.Client;
using Nethereum.Web3;
using nft.contract.query.Queries;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

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
            try
            {
                IClient clinet = new RpcClient(new Uri("HTTP://127.0.0.1:7545"));

                Web3 d = new Web3(clinet);
                var h = d.Eth.GetContractHandler("0xe46dab9FA9ecb10da980c7BE8138a26013153eF3");

                var balanceOfFunction = new BalanceOfQuery()
                {
                    Owner = address
                };

                var result = await _ContractHandler.QueryAsync<BalanceOfQuery, int>(balanceOfFunction);
                return result;
            }
            catch (Exception ex)
            {
                return 0;
            }
        }
    }
}
