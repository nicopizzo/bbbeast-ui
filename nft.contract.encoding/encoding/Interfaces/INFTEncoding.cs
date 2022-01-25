using NFT.Contract.Models;

namespace NFT.Contract.Encoding
{
    public interface INFTEncoding
    {
        EncodingResult GetMintFunctionEncoding(int mintCount);
        EncodingResult GetPrivateSaleMintFunctionEncoding(int mintCount);
    }
}