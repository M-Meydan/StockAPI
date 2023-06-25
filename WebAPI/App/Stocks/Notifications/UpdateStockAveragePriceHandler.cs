using MediatR;
using Microsoft.Extensions.Logging;
using System.Threading;
using System.Threading.Tasks;
using WebAPI.App.Trades.Notifications;
using WebAPI.Infrastructure.Repositories;

namespace WebAPI.App.Stocks.Notifications
{
    public class UpdateStockAveragePriceHandler : INotificationHandler<TradeCreatedNotification>
    {
        private readonly IStockRepository _stockRepository;
        private readonly ITradeRepository _tradeRepository;
        private readonly ILogger<UpdateStockAveragePriceHandler> _logger;

        public UpdateStockAveragePriceHandler(IStockRepository stockRepository,
             ITradeRepository tradeRepository,
            ILogger<UpdateStockAveragePriceHandler> logger)
        {
            _stockRepository = stockRepository;
            _tradeRepository = tradeRepository;
            _logger = logger;
        }

        public async Task Handle(TradeCreatedNotification notification, CancellationToken cancellationToken)
        {
            var stock = await _stockRepository.GetAsync(notification.Symbol);

            if (stock == null)
            {
                _logger.LogError($"Stock not found for {notification.Symbol}");
                return;
            };

            var averagePrice = _tradeRepository.GetAverageStockPrice(notification.Symbol);
            stock.CurrentPrice = averagePrice;

            await _stockRepository.Update(stock);
        }
    }



}
