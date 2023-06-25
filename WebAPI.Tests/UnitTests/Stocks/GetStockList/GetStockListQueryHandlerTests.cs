using AutoMapper;
using Moq;
using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using WebAPI.App.Stocks.Queries.GetStock;
using WebAPI.App.Stocks.Queries.GetStockList;
using WebAPI.Domain.Entities;

namespace WebAPI.Tests.UnitTests.Stocks.GetStockList
{
    [TestFixture]
    public class GetStockListQueryHandlerTests
    {
        private Mock<IStockRepository> _stockRepositoryMock;
        private IMapper _mapper;
        private GetStockListQueryHandler _handler;

        [SetUp]
        public void Setup()
        {
            _stockRepositoryMock = new Mock<IStockRepository>();
            _mapper = new MapperConfiguration(cfg => { cfg.CreateMap<Stock, StockDto>(); }).CreateMapper();
            _handler = new GetStockListQueryHandler(_stockRepositoryMock.Object, _mapper);
        }

        [TestCase(null)]
        [TestCase("")]
        public async Task Handle_NoSymbols_ReturnsAllStocks_AsPaginatedList(string symbol)
        {
            // Arrange
            var query = new GetStockListQuery { Symbols = symbol };
            var stocks = GetTestStocks();

            _stockRepositoryMock.Setup(repo => repo.GetQueryable()).Returns(stocks.AsQueryable());

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            var stockCount = GetTestStocks().Count();
            Assert.AreEqual(stockCount, result.Data.Count);
            Assert.AreEqual(1, result.PageNumber);
            Assert.AreEqual(stockCount, result.TotalCount);
        }

        [TestCase("vod", 1)]
        [TestCase("vod,nwg", 2)]
        [TestCase("vod,nwg,shel", 3)]
        public async Task Handle_WithSymbols_ReturnsStocks_AsPaginatedList(string symbol, int expectedDataCount)
        {
            // Arrange
            var query = new GetStockListQuery { Symbols = symbol };
            var stocks = GetTestStocks();

            _stockRepositoryMock.Setup(repo => repo.GetQueryable()).Returns(stocks.AsQueryable());

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.AreEqual(expectedDataCount, result.Data.Count);
            Assert.AreEqual(query.GetSymbols(), result.Data.Select(x => x.Symbol).ToList());
        }

        [TestCase(2, 2, 1)]
        [TestCase(1, 2, 2)]
        [TestCase(1, 1, 1)]
        public async Task Handle_ReturnsAllStocks_AsPaginatedList(int pageNumber, int pageSize, int expectedDataCount)
        {
            // Arrange
            var query = new GetStockListQuery { PageNumber = pageNumber, PageSize = pageSize };
            var stocks = GetTestStocks();

            _stockRepositoryMock.Setup(repo => repo.GetQueryable()).Returns(stocks.AsQueryable());

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.AreEqual(pageNumber, result.PageNumber);
            Assert.AreEqual(expectedDataCount, result.Data.Count);
        }

        private IEnumerable<Stock> GetTestStocks()
        {
            var stocks = new List<Stock>
        {
            new Stock("vod","Vodafone Group",0m),
            new Stock("nwg","National Westminster Bank",0m),
            new Stock("shel","Shell",0m)
        };

            return stocks;
        }
    }

}

