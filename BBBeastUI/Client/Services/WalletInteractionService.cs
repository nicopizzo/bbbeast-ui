namespace BBBeastUI.Services
{
    public interface IWalletInteractionService
    {
        event Action RefreshRequested;
        void CallRequestRefresh();
    }

    public class WalletInteractionService : IWalletInteractionService
    {
        public event Action RefreshRequested;

        public void CallRequestRefresh()
        {
            RefreshRequested?.Invoke();
        }
    }
}
