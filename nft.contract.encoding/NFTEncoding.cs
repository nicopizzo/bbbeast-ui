using Nethereum.ABI.FunctionEncoding;
using Nethereum.ABI.Model;
using nft.contract.encoding.Parameters;

namespace nft.contract.encoding
{
    public class NFTEncoding : INFTEncoding
    {
        public string GetMintFunctionEncoding(int mintCount)
        {
            FunctionABI function = new FunctionABI("mint", false);
            return CreateMintEncoding(function, mintCount);
        }

        public string GetPrivateSaleMintFunctionEncoding(int mintCount)
        {
            FunctionABI function = new FunctionABI("mint", false);
            return CreateMintEncoding(function, mintCount);
        }

        private string CreateMintEncoding(FunctionABI function, int mintCount)
        {
            var parameters = new MintParameters() { MintCount = mintCount };
            function.InputParameters = parameters.GetParameters();

            var functionCallEncoder = new FunctionCallEncoder();
            var data = functionCallEncoder.EncodeRequest(function.Sha3Signature, function.InputParameters, parameters.GetParameterValues());
            return data;
        }
    }
}
