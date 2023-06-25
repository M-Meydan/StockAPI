using MediatR;
using System.Collections.Generic;
using System.Linq;
using WebAPI.App.Models;
using WebAPI.App.Stocks.Queries.GetStock;

namespace WebAPI.App.Stocks.Queries.GetStockList
{
    public class GetStockListQuery : IRequest<PaginatedList<StockDto>>
    {
        /// <summary>
        /// Comma separated ticker symbols used for filtering
        /// e.g. "vod or vod,nwg,shel"
        /// </summary>
        /// <example>vod</example>
        public string Symbols { get; set; }

        /// <summary>
        /// Page number for pagination. Default = 1.
        /// </summary>
        /// <example>1</example>
        public int PageNumber { get; set; } = 1;

        /// <summary>
        /// Number of items per page for pagination. Default =10
        /// </summary>
        /// <example>10</example>
        public int PageSize { get; set; } = 10;

        public GetStockListQuery() { }

        /// <summary>
        /// Symbols as list
        /// </summary>
        public List<string> GetSymbols()
        {
            return !string.IsNullOrEmpty(Symbols)
                       ? Symbols.Split(',').Select(s => s.Trim()).ToList()
                       : new List<string>(0);
        }
    }

}
