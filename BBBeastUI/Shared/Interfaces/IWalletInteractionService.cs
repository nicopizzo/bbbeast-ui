namespace BBBeast.UI.Shared.Interfaces
{
    public interface IWalletInteractionService
    {
        event Func<Task> RefreshRequested;
        void CallRequestRefresh();
    }
}
