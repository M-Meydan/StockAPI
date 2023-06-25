using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using WebAPI.App.Models;
using WebAPI.Controllers;
using WebAPI.Tests.IntegrationTests.Infrastructure;

namespace WebAPI.Tests.IntegrationTests.MiddlewareTests
{
    [TestFixture]
    public class ApiKeyMiddlewareTests
    {
        private readonly APIApplication<StocksController> _application;
        private readonly HttpClient _httpClient;
        private readonly ApiSettings _apiSettings;

        public ApiKeyMiddlewareTests()
        {
            _application = new APIApplication<StocksController>();
            _httpClient = _application.CreateClient();
            _apiSettings  = _application.Services.GetService<ApiSettings>();
        }

        public void Dispose()
        {
            _httpClient.Dispose();
            _application.Dispose();
        }

        [Test]
        public async Task ApiKeyMiddleware_ReturnsAuthorized_WhenValidApiKeyProvided()
        {
            // Arrange
            var request = new HttpRequestMessage(HttpMethod.Get, "/api/stocks/tickers/vod");
            request.Headers.Add("X-Api-Key", _apiSettings.ApiKey);

            // Act
            var response = await _httpClient.SendAsync(request);

            // Assert
            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
        }

        [Test]
        public async Task ApiKeyMiddleware_ReturnsUnauthorized_WhenInvalidApiKeyProvided()
        {
            var request = new HttpRequestMessage(HttpMethod.Get, "/api/stocks/tickers/vod");
            request.Headers.Add("X-Api-Key", "invalidkey");

            // Act
            var response = await _httpClient.SendAsync(request);

            // Assert
            Assert.AreEqual(HttpStatusCode.Unauthorized, response.StatusCode);
        }

    }
}
