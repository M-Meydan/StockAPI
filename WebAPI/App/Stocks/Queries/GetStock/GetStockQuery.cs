using MediatR;

namespace WebAPI.App.Stocks.Queries.GetStock
{
    public class GetStockQuery : IRequest<StockDto>
    {
        /// <summary>
        /// Ticker symbol
        /// </summary>
        /// <example>vod</example>
        public string Symbol { get; set; }

        public GetStockQuery() { }
        public GetStockQuery(string symbol)
        {
            Symbol = symbol;
        }
    }

}
