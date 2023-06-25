using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using NUnit.Framework;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using WebAPI.App.Models;
using WebAPI.App.Stocks.Queries.GetStock;
using WebAPI.Controllers;
using WebAPI.Infrastructure;
using WebAPI.Tests.IntegrationTests.Infrastructure;

namespace WebAPI.Tests.IntegrationTests.Controllers
{
    [TestFixture]
    public class StocksControllerTests
    {
        private readonly APIApplication<StocksController> _application;
        private readonly HttpClient _httpClient;
        private readonly ApiSettings _apiSettings;
        private readonly AppDbContext _appDbContext;

        public StocksControllerTests()
        {
            _application = new APIApplication<StocksController>();
            _httpClient = _application.CreateClient();
            _apiSettings = _application.Services.GetService<ApiSettings>();
            _httpClient.DefaultRequestHeaders.Add("X-Api-Key", _apiSettings.ApiKey);
        }

        [Test]
        public async Task GetStock_ExistingSymbol_ReturnsStockData()
        {
            // Arrange
            var symbol = "vod";
            var endpoint = $"/api/stocks/tickers/{symbol}";

            // Act
            var response = await _httpClient.GetAsync(endpoint);

            // Assert
            response.EnsureSuccessStatusCode();
            var stock = await response.Content.ReadFromJsonAsync<StockDto>();
            Assert.NotNull(stock);
            Assert.AreEqual(symbol, stock.Symbol);
        }

        [Test]
        public async Task GetStock_NonExistentSymbol_ReturnsNotFound()
        {
            // Arrange
            var symbol = "INVALID";
            var endpoint = $"/api/stocks/tickers/{symbol}";

            // Act
            var response = await _httpClient.GetAsync(endpoint);

            // Assert
            Assert.AreEqual(HttpStatusCode.NotFound, response.StatusCode);
        }

        [Test]
        public async Task GetStockList_NoSymbols_ReturnsAllStocks()
        {
            // Arrange
            var endpoint = "/api/stocks/tickers";

            // Act
            var response = await _httpClient.GetAsync(endpoint);

            // Assert
            response.EnsureSuccessStatusCode();
            var content = await response.Content.ReadAsStringAsync();
            var resultObj = JsonConvert.DeserializeObject<PaginatedList<StockDto>>(content);

            Assert.NotNull(resultObj);
            Assert.AreEqual(DataSeeder.StockFeed.Count, resultObj.Data.Count);
        }

        [Test]
        public async Task GetStockList_RangeOfSymbols_ReturnsMatchingStocks()
        {
            // Arrange
            var symbols = "vod,nwg,shel";
            var endpoint = $"/api/stocks/tickers?symbols={symbols}";

            // Act
            var response = await _httpClient.GetAsync(endpoint);

            // Assert
            response.EnsureSuccessStatusCode();
            var content = await response.Content.ReadAsStringAsync();
            var resultObj = JsonConvert.DeserializeObject<PaginatedList<StockDto>>(content);

            var requestedSymbols = symbols.Split(',');
            var actualSymbols = resultObj.Data.Select(stock => stock.Symbol);
            
            Assert.NotNull(resultObj);
            Assert.That(actualSymbols, Is.EquivalentTo(requestedSymbols));
        }

        [Test]
        public async Task GetStockList_ReturnsPaginatedStocks()
        {
            // Arrange
            var endpoint = "/api/stocks/tickers";
            var pageNumber = 2;
            var pageSize = 1;

            // Act
            var response = await _httpClient.GetAsync($"{endpoint}?pageNumber={pageNumber}&pageSize={pageSize}");

            // Assert
            response.EnsureSuccessStatusCode();
            var content = await response.Content.ReadAsStringAsync();
            var resultObj = JsonConvert.DeserializeObject<PaginatedList<StockDto>>(content);

            Assert.NotNull(resultObj);
            Assert.AreEqual(pageNumber, resultObj.PageNumber);
            Assert.AreEqual(pageSize, resultObj.Data.Count);
            Assert.GreaterOrEqual(resultObj.TotalCount, resultObj.Data.Count);
            Assert.AreEqual(pageSize, resultObj.Data.Count);
        }

    }
}
