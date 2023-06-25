using FluentValidation;
using FluentValidation.Results;
using System.Threading;
using System.Threading.Tasks;

namespace WebAPI.App.Trades.Commands.CreateTrade
{
    public class CreateTradeCommandValidator : AbstractValidator<CreateTradeCommand>
    {
        private readonly IStockRepository _stockRepository;

        public CreateTradeCommandValidator(IStockRepository stockRepository)
        {
            _stockRepository = stockRepository;

            RuleFor(x => x.Symbol)
                .NotEmpty().WithMessage("Ticker symbol is required.")
                .MustAsync(ExistInDatabase).WithMessage("Stock does not exist in the database.");

            RuleFor(x => x.PriceInPound)
                .NotEmpty().WithMessage("Price in pound is required.")
                .GreaterThan(0).WithMessage("Price must be greater than zero.");

            RuleFor(x => x.NumberOfShares)
                .NotEmpty().WithMessage("Number of shares is required.")
                .GreaterThan(0).WithMessage("Number of shares must be greater than zero.");

            RuleFor(x => x.BrokerId)
                .NotEmpty().WithMessage("Broker ID is required.")
                .GreaterThan(0).WithMessage("Broker ID must be greater than zero.");
        }

        private async Task<bool> ExistInDatabase(string symbol, CancellationToken cancellationToken)
        {
            return await _stockRepository.Exists(symbol);
        }

        protected override bool PreValidate(ValidationContext<CreateTradeCommand> context, ValidationResult result)
        {
            if (context.InstanceToValidate == null)
            {
                result.Errors.Add(new ValidationFailure(nameof(CreateTradeCommand), "Provide a valid model."));
                return false;
            }
            return base.PreValidate(context, result);
        }
    }

}
