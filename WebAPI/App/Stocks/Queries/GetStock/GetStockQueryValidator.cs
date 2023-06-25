using FluentValidation;
using FluentValidation.Results;

namespace WebAPI.App.Stocks.Queries.GetStock
{
    public class GetStockQueryValidator : AbstractValidator<GetStockQuery>
    {
        public GetStockQueryValidator()
        {
            CascadeMode = CascadeMode.Stop;

            RuleFor(x => x.Symbol)
                .NotNull().NotEmpty().WithMessage("Ticker symbol is required.");

            RuleFor(x => x.Symbol.Length)
                .GreaterThan(0).WithMessage("Symbol must have at least one character.");
        }
        protected override bool PreValidate(ValidationContext<GetStockQuery> context, ValidationResult result)
        {
            if (context.InstanceToValidate == null)
            {
                result.Errors.Add(new ValidationFailure(nameof(GetStockQuery), "Provide a valid model."));
                return false;
            }
            return base.PreValidate(context, result);
        }
    }

}
