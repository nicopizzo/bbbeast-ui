using Nethereum.ABI.FunctionEncoding.Attributes;
using Nethereum.Contracts;

namespace NFT.Contract.Query.Queries
{
    [Function("maxSupply", "uint256")]
    public class MaxSupplyQuery : FunctionMessage
    {
    }
}
