using NUnit.Framework;
using WebAPI.App.Stocks.Queries.GetStock;

namespace WebAPI.Tests.UnitTests.Stocks.GetStock
{
    [TestFixture]
    public class GetStockQueryValidatorTests
    {
        private GetStockQueryValidator _validator;

        [SetUp]
        public void Setup()
        {
            _validator = new GetStockQueryValidator();
        }

        [TestCase("A")]
        [TestCase("AL")]
        [TestCase("AAPL")]
        public void Validate_ValidQuery_ShouldPassValidation(string symbol)
        {
            // Arrange
            var query = new GetStockQuery { Symbol = symbol };

            // Act
            var result = _validator.Validate(query);

            // Assert
            Assert.IsTrue(result.IsValid);
        }

        [TestCase(null, TestName= "NullSymbol")]
        [TestCase("", TestName= "EmptySymbol")]
        [TestCase(" ", TestName= "ZeroLengthSymbol")]
        public void Validate_InvalidSymbols_ShouldFailValidation(string symbol)
        {
            // Arrange
            var query = new GetStockQuery { Symbol = symbol };

            // Act
            var result = _validator.Validate(query);

            // Assert
            Assert.IsFalse(result.IsValid);
        }

        [Test]
        public void Validate_NullQuery_ShouldFailValidation()
        {
            // Arrange
            GetStockQuery query = null;

            // Act
            var result = _validator.Validate(query);

            // Assert
            Assert.IsFalse(result.IsValid);
        }
    }

}
