using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using WebAPI.App.Mappings;
using WebAPI.App.Models;
using WebAPI.App.Stocks.Queries.GetStock;

namespace WebAPI.App.Stocks.Queries.GetStockList
{
    public class GetStockListQueryHandler : IRequestHandler<GetStockListQuery, PaginatedList<StockDto>>
    {
        private readonly IStockRepository _stockRepository;
        private readonly IMapper _mapper;

        public GetStockListQueryHandler(IStockRepository stockRepository, IMapper mapper)
        {
            _stockRepository = stockRepository;
            _mapper = mapper;
        }

        public Task<PaginatedList<StockDto>> Handle(GetStockListQuery request, CancellationToken cancellationToken)
        {
            var symbols = request.GetSymbols();
            var queryable = _stockRepository.GetQueryable();

            if (symbols.Count > 0)
                queryable = queryable.Where(stock => symbols.Contains(stock.Symbol));

            var result = queryable
             .ProjectTo<StockDto>(_mapper.ConfigurationProvider)
             .PaginatedList(request.PageNumber, request.PageSize);

            return Task.FromResult(result);
        }
    }
}
