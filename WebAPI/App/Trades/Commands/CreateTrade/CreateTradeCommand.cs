using MediatR;

namespace WebAPI.App.Trades.Commands.CreateTrade
{
    /// <summary>
    /// Holds transaction information
    /// </summary>
    public class CreateTradeCommand : IRequest<int>
    {
        /// <summary>
        /// Ticker symbol
        /// </summary>
        public string Symbol { get; set; }
        public decimal PriceInPound { get; set; }
        public decimal NumberOfShares { get; set; }

        public int BrokerId { get; set; }
    }

}