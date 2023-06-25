using AutoMapper;
using FluentValidation;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace WebAPI.App.Stocks.Queries.GetStock
{
    public class GetStockQueryHandler : IRequestHandler<GetStockQuery, StockDto>
    {
        private readonly IStockRepository _stockRepository;
        private readonly IValidator<GetStockQuery> _validator;
        private readonly IMapper _mapper;

        public GetStockQueryHandler(IStockRepository stockRepository,
            IValidator<GetStockQuery> validator, 
            IMapper mapper)
        {
            _stockRepository = stockRepository;
            _validator = validator;
            _mapper = mapper;
        }

        public async Task<StockDto> Handle(GetStockQuery query, CancellationToken cancellationToken)
        {
            await _validator.ValidateAndThrowAsync(query, cancellationToken);

            var stock = await _stockRepository.GetAsync(query.Symbol);
            return _mapper.Map<StockDto>(stock);
        }
    }
}
