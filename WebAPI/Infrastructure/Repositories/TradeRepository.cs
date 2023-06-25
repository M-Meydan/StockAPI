using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using WebAPI.Domain.Entities;

namespace WebAPI.Infrastructure.Repositories
{
    public interface ITradeRepository
    {
        Task<int> CreateAsync(Trade trade, CancellationToken cancellationToken = default);
        decimal GetAverageStockPrice(string symbol);
    }

    public class TradeRepository : ITradeRepository
    {
        private readonly AppDbContext _dbContext;

        public TradeRepository(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<int> CreateAsync(Trade trade, CancellationToken cancellationToken = default)
        {
            _dbContext.Trades.Add(trade);
            await _dbContext.SaveChangesAsync(cancellationToken);
            return trade.Id;
        }

        public decimal GetAverageStockPrice(string symbol)
        {
            var averagePrice = _dbContext.Trades
                                .Where(t => t.Symbol == symbol)
                                .Average(t => t.PriceInPound);

            return averagePrice;
        }
    }

}
