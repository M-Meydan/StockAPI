using WebAPI.App.Mappings;
using WebAPI.Domain.Entities;

namespace WebAPI.App.Stocks.Queries.GetStock
{
    public class StockDto : IMapFrom<Stock>
    {
        public string Symbol { get; set; }
        public decimal CurrentPrice { get; set; }
        public StockDto() { }
    }
}