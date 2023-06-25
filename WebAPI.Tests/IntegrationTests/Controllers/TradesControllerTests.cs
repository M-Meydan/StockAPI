using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using NUnit.Framework;
using System.Net;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using WebAPI.App.Models;
using WebAPI.App.Trades.Commands.CreateTrade;
using WebAPI.Controllers;
using WebAPI.Tests.IntegrationTests.Infrastructure;

namespace WebAPI.Tests.IntegrationTests.Controllers
{
    [TestFixture]
    public class TradesControllerTests
    {
        private readonly APIApplication<TradesController> _application;
        private readonly HttpClient _httpClient;
        private readonly ApiSettings _apiSettings;

        public TradesControllerTests()
        {
            _application = new APIApplication<TradesController>();
            _httpClient = _application.CreateClient();
            _apiSettings = _application.Services.GetService<ApiSettings>();
            _httpClient.DefaultRequestHeaders.Add("X-Api-Key", _apiSettings.ApiKey);
        }

       
        [Test]
        public async Task CreateTrade_ValidRequest_ReturnsTradeId()
        {
            // Arrange
            var request = new CreateTradeCommand
            {
                Symbol = "vod",
                PriceInPound = 100.5m,
                NumberOfShares = 10,
                BrokerId = 1
            };

            // Act
            var response = await _httpClient.PostAsJsonAsync($"/api/trades", request);

            // Assert
            
            var responseContent = await response.Content.ReadAsStringAsync();
            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
            Assert.IsTrue(int.Parse(responseContent) >0);
        }


        [Test]
        public async Task CreateTrade_InvalidRequest_ReturnsBadRequest()
        {
            // Arrange
            var request = new CreateTradeCommand
            {
                Symbol = null,
                PriceInPound = 0,
                NumberOfShares = 0,
                BrokerId = 0
            };

            // Act
            var response = await _httpClient.PostAsJsonAsync($"/api/trades", request);

            // Assert
            var responseContent = await response.Content.ReadAsStringAsync();
            var problemDetails = JsonConvert.DeserializeObject<ValidationProblemDetails>(responseContent);
            
            Assert.AreEqual(HttpStatusCode.BadRequest, response.StatusCode);
            Assert.NotNull(problemDetails.Title, "Validation error");
            Assert.IsTrue(problemDetails.Errors.Count > 0);
        }
    }

}
