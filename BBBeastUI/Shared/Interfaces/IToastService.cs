using BBBeast.UI.Shared.Models;

namespace BBBeast.UI.Shared.Interfaces
{
    public interface IToastService
    {
        void PublishMessage(string message, ToastType type);
    }
}