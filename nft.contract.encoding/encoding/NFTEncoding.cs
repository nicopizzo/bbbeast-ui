using Nethereum.ABI.FunctionEncoding;
using Nethereum.ABI.Model;
using NFT.Contract.Encoding.Parameters;
using NFT.Contract.Models;

namespace NFT.Contract.Encoding
{
    public class NFTEncoding : INFTEncoding
    {
        public EncodingResult GetMintFunctionEncoding(int mintCount)
        {
            FunctionABI function = new FunctionABI("mint", false);
            return CreateMintEncoding(function, mintCount);
        }

        public EncodingResult GetPrivateSaleMintFunctionEncoding(int mintCount)
        {
            FunctionABI function = new FunctionABI("privateSaleMint", false);
            return CreateMintEncoding(function, mintCount);
        }

        private EncodingResult CreateMintEncoding(FunctionABI function, int mintCount)
        {
            var parameters = new MintParameters() { MintCount = mintCount };
            function.InputParameters = parameters.GetParameters();

            var functionCallEncoder = new FunctionCallEncoder();
            var data = functionCallEncoder.EncodeRequest(function.Sha3Signature, function.InputParameters, parameters.GetParameterValues());
            return new EncodingResult() { Result = data };
        }
    }
}
