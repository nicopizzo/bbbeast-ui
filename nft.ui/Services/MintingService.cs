namespace nft.ui.Services
{
    public interface IMintingService
    {
        Task MintNFT(int count);
    }

    public class MintingService : IMintingService
    {
        public Task MintNFT(int count)
        {
            throw new NotImplementedException();
        }
    }
}
