using Nethereum.ABI.FunctionEncoding.Attributes;

namespace NFT.Contract.Encoding.Parameters
{
    public class MintParameters : ParameterBase
    {
        [Parameter("uint256", "mintCount")]
        public int MintCount { get; set; }
    }
}
