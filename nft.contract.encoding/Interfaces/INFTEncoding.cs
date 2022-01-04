namespace nft.contract.encoding
{
    public interface INFTEncoding
    {
        string GetMintFunctionEncoding(int mintCount);
        string GetPrivateSaleMintFunctionEncoding(int mintCount);
    }
}