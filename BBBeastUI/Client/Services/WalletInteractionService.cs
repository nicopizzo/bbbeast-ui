using BBBeast.UI.Shared.Interfaces;

namespace BBBeastUI.Services
{
    public class WalletInteractionService : IWalletInteractionService
    {
        public event Func<Task> RefreshRequested;

        public void CallRequestRefresh()
        {
            RefreshRequested?.Invoke();
        }
    }
}
