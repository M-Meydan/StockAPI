using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;
using WebAPI.Domain.Entities;
using WebAPI.Infrastructure;

public interface IStockRepository
{
    Task<Stock> GetAsync(string symbol);
    Task<bool> Exists(string symbol);

    IQueryable<Stock> GetQueryable();

    Task Update(Stock stock);
}

public class StockRepository : IStockRepository
{
    private readonly AppDbContext _dbContext;

    public StockRepository(AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<Stock> GetAsync(string symbol)
    {
        return await _dbContext.Stocks.FirstOrDefaultAsync(s => s.Symbol == symbol);
    }

    public async Task<bool> Exists(string symbol)
    {
        return await _dbContext.Stocks.AnyAsync(stock => stock.Symbol == symbol);
    }

    /// <summary>
    /// Used for pagination queries
    /// </summary>
    /// <returns></returns>
    public IQueryable<Stock> GetQueryable() => _dbContext.Stocks;

    public async Task Update(Stock stock)
    {
        _dbContext.Stocks.Update(stock);
        await _dbContext.SaveChangesAsync();
    }
}
