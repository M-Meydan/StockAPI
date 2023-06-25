using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using WebAPI.App.Trades.Commands.CreateTrade;

namespace WebAPI.Controllers
{
    /// <summary>
    /// Trade APIs
    /// </summary>
    [Route("api/trades")]
    [ApiController]
    public class TradesController : ControllerBase
    {
        private readonly IMediator _mediator;

        public TradesController(IMediator mediator)
        {

            _mediator = mediator;
        }

        /// <summary>
        /// Receives trade notifications from authorised brokers.
        /// </summary>
        /// <param name="command">transaction information</param>
        /// <returns>transaction id</returns>
        [HttpPost()]
        [ProducesResponseType(typeof(int), StatusCodes.Status200OK)]
        public async Task<int> PostTrades(CreateTradeCommand command)
        {
            return await _mediator.Send(command);
        }
    }
}
