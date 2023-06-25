using AutoMapper;
using FluentValidation;
using FluentValidation.Results;
using Moq;
using NUnit.Framework;
using System.Threading;
using System.Threading.Tasks;
using WebAPI.App.Stocks.Queries.GetStock;
using WebAPI.Domain.Entities;

namespace WebAPI.Tests.UnitTests.Stocks.GetStock
{
    [TestFixture]
    public class GetStockQueryHandlerTests
    {
        private Mock<IStockRepository> _stockRepositoryMock;
        private Mock<IValidator<GetStockQuery>> _validatorMock;
        private Mock<IMapper> _mapperMock;
        private GetStockQueryHandler _handler;

        [SetUp]
        public void Setup()
        {
            _stockRepositoryMock = new Mock<IStockRepository>();
            _validatorMock = new Mock<IValidator<GetStockQuery>>();
            _mapperMock = new Mock<IMapper>();
            _handler = new GetStockQueryHandler(_stockRepositoryMock.Object, _validatorMock.Object, _mapperMock.Object);
        }

        [Test]
        public async Task Handle_ValidQuery_ReturnsStockDto()
        {
            // Arrange
            var query = new GetStockQuery { Symbol = "AAPL" };
            var cancellationToken = CancellationToken.None;
            var stock = new Stock("AAPL", "AAPL",0M);
            var expectedDto = new StockDto { Symbol = "AAPL", CurrentPrice = 100.0m };

            _validatorMock.Setup(x => x.ValidateAsync(It.IsAny<ValidationContext<GetStockQuery>>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new ValidationResult());

            _stockRepositoryMock.Setup(x => x.GetAsync(query.Symbol))
                .ReturnsAsync(stock);

            _mapperMock.Setup(x => x.Map<StockDto>(stock))
                .Returns(expectedDto);

            // Act
            var result = await _handler.Handle(query, cancellationToken);

            // Assert
            Assert.AreEqual(expectedDto, result);
        }
    }


}
