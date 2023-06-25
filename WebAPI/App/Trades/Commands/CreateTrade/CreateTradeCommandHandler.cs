using AutoMapper;
using FluentValidation;
using MediatR;
using System.Threading;
using System.Threading.Tasks;
using WebAPI.App.Trades.Notifications;
using WebAPI.Domain.Entities;
using WebAPI.Infrastructure.Repositories;

namespace WebAPI.App.Trades.Commands.CreateTrade
{
    public class CreateTradeCommandHandler : IRequestHandler<CreateTradeCommand, int>
    {
        private readonly ITradeRepository _transactionRepository;
        private readonly IStockRepository _stockRepository;
        private readonly IValidator<CreateTradeCommand> _validator;
        private readonly IMapper _mapper;
        private readonly IMediator _mediator;

        public CreateTradeCommandHandler(ITradeRepository transactionRepository,
            IStockRepository stockRepository,
            IValidator<CreateTradeCommand> validator,
            IMapper mapper,
            IMediator mediator)
        {
            _transactionRepository = transactionRepository;
            _stockRepository = stockRepository;
            _validator = validator;
            _mapper = mapper;
            _mediator = mediator;
        }

        public async Task<int> Handle(CreateTradeCommand request, CancellationToken cancellationToken)
        {
            await _validator.ValidateAndThrowAsync(request,cancellationToken);

            var entity = _mapper.Map<Trade>(request);

            var tradeId = await _transactionRepository.CreateAsync(entity, cancellationToken);

            await _mediator.Publish(new TradeCreatedNotification { Symbol = entity.Symbol }, cancellationToken);

            return tradeId;
        }
    }

}