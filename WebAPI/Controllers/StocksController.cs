using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using WebAPI.App.Models;
using WebAPI.App.Stocks.Queries.GetStock;
using WebAPI.App.Stocks.Queries.GetStockList;

namespace WebAPI.Controllers
{
    /// <summary>
    /// Stock APIs
    /// </summary>
    [Route("api/stocks")]
    [ApiController]
    public class StocksController : ControllerBase
    {
        private readonly IMediator _mediator;

        public StocksController(IMediator mediator)
        {
            _mediator = mediator;
        }

        /// <summary>
        /// Returns stock by a ticker symbol
        /// </summary>
        /// <returns>stock data</returns>
        [HttpGet("tickers/{symbol}")]
        [ProducesResponseType(typeof(StockDto), StatusCodes.Status200OK)]
        public async Task<ActionResult<StockDto>> GetStock([FromRoute] GetStockQuery query)
        {
            var result = await _mediator.Send(query);

            if (result == null)
                return NotFound();

            return result;
        }

        /// <summary>
        /// Returns all stocks if no symbols provided
        /// </summary>
        /// <returns>List of stock data</returns>
        [HttpGet("tickers")]
        public async Task<ActionResult<PaginatedList<StockDto>>> GetStockList([FromQuery] GetStockListQuery query)
        {
            return await _mediator.Send(query);
        }
    }
}
