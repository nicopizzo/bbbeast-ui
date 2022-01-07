using Nethereum.ABI.FunctionEncoding.Attributes;
using Nethereum.Contracts;

namespace nft.contract.query.Queries
{
    [Function("balanceOf", "uint256")]
    internal class BalanceOfQuery : FunctionMessage
    {
        [Parameter("address", "owner", 1)]
        public virtual string Owner { get; set; }
    }
}
