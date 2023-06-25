using Moq;
using NUnit.Framework;
using System.Threading.Tasks;
using WebAPI.App.Trades.Commands.CreateTrade;

namespace WebAPI.Tests.UnitTests.Trades.CreateTrade
{
    [TestFixture]
    public class CreateTradeCommandValidatorTests
    {
        private CreateTradeCommandValidator _validator;
        private Mock<IStockRepository> _stockRepositoryMock;

        [SetUp]
        public void Setup()
        {
            _stockRepositoryMock = new Mock<IStockRepository>();
            _validator = new CreateTradeCommandValidator(_stockRepositoryMock.Object);
        }

        [Test]
        public async Task Validate_ValidCommand_ReturnsValidResult()
        {
            // Arrange
            var command = new CreateTradeCommand
            {
                Symbol = "AAPL",
                PriceInPound = 100,
                NumberOfShares = 10,
                BrokerId = 1
            };

            _stockRepositoryMock.Setup(x => x.Exists(command.Symbol)).ReturnsAsync(true);

            // Act
            var result = await _validator.ValidateAsync(command);

            // Assert
            Assert.IsTrue(result.IsValid);
        }

        [Test]
        public async Task Validate_InvalidCommand_ReturnsValidationErrors()
        {
            // Arrange
            var symbol = "";
            var priceInPound = 0;
            var numberOfShares = 0;
            var brokerId = 0;

            var command = new CreateTradeCommand
            {
                Symbol = symbol,
                PriceInPound = priceInPound,
                NumberOfShares = numberOfShares,
                BrokerId = brokerId
            };

            _stockRepositoryMock.Setup(x => x.Exists(symbol)).ReturnsAsync(false);

            // Act
            var result = await _validator.ValidateAsync(command);

            // Assert
            Assert.IsFalse(result.IsValid);
            Assert.IsNotEmpty(result.Errors);
        }

        [Test]
        public void Validate_NullQuery_ShouldFailValidation()
        {
            // Arrange
            CreateTradeCommand query = null;

            // Act
            var result = _validator.Validate(query);

            // Assert
            Assert.IsFalse(result.IsValid);
        }
    }

}
