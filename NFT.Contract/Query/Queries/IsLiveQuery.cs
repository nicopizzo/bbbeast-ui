using Nethereum.ABI.FunctionEncoding.Attributes;
using Nethereum.Contracts;

namespace NFT.Contract.Query.Queries
{
    [Function("isLive", "bool")]
    internal class IsLiveQuery : FunctionMessage
    {
    }
}
