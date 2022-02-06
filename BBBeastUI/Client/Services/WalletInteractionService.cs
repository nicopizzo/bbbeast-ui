namespace BBBeastUI.Services
{
    public interface IWalletInteractionService
    {
        event Func<Task> RefreshRequested;
        void CallRequestRefresh();
    }

    public class WalletInteractionService : IWalletInteractionService
    {
        public event Func<Task> RefreshRequested;

        public void CallRequestRefresh()
        {
            RefreshRequested?.Invoke();
        }
    }
}
