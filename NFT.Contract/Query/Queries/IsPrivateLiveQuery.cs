using Nethereum.ABI.FunctionEncoding.Attributes;
using Nethereum.Contracts;

namespace NFT.Contract.Query.Queries
{
    [Function("privateSaleLive", "bool")]
    internal class IsPrivateLiveQuery : FunctionMessage
    {
    }
}
