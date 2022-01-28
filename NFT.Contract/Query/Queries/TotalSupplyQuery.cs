using Nethereum.ABI.FunctionEncoding.Attributes;
using Nethereum.Contracts;

namespace NFT.Contract.Query.Queries
{
    [Function("totalSupply", "uint256")]
    internal class TotalSupplyQuery : FunctionMessage
    {
    }
}
