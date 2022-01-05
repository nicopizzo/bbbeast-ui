using Nethereum.ABI.FunctionEncoding.Attributes;

namespace nft.contract.encoding.Parameters
{
    public class MintParameters : ParameterBase
    {
        [Parameter("uint256", "mintCount")]
        public int MintCount { get; set; }
    }
}
