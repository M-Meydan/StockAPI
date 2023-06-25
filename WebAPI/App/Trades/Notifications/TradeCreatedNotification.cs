using MediatR;

namespace WebAPI.App.Trades.Notifications
{
    public class TradeCreatedNotification : INotification
    {
        public string Symbol { get; set; }
    }
}