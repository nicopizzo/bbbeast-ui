using BBBeastUI.Models;
using Havit.Blazor.Components.Web;

namespace BBBeastUI.Services
{
    public interface IToastService
    {
        void PublishMessage(string message, ToastType type);
    }

    public class ToastService : IToastService
    {
        private readonly IHxMessengerService _MessengerService;

        public ToastService(IHxMessengerService messengerService)
        {
            _MessengerService = messengerService;
        }

        public void PublishMessage(string message, ToastType type)
        {
            var tmes = CreateMessage(message, type);
            _MessengerService.AddMessage(tmes);
        }

        private MessengerMessage CreateMessage(string message, ToastType type)
        {
            var icon = FontAwesomeIcon.CircleQuestion;
            var css = string.Empty;
            var delay = 2000;
            switch (type)
            {
                case ToastType.Success:
                    icon = FontAwesomeIcon.Check;
                    css = "toast-success";
                    delay = 10000;
                    break;
                case ToastType.Warning:
                    icon = FontAwesomeIcon.TriangleExclamation;
                    css = "toast-warning";
                    break;
                case ToastType.Failure:
                    icon = FontAwesomeIcon.CircleExclamation;
                    css = "toast-failure";
                    break;
            }
            return new MessengerMessage() { AutohideDelay = delay, Title = message, Icon = icon, CssClass = css };
        }
    }
}
