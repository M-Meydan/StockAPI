using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using WebAPI.App.Models;

namespace WebAPI.App.Middlewares
{
    public class ApiKeyMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ApiSettings _apiSettings;

        public ApiKeyMiddleware(RequestDelegate next, ApiSettings apiSettings)
        {
            _next = next;
            _apiSettings = apiSettings;
        }

        public async Task Invoke(HttpContext context)
        {
            if (!context.Request.Headers.TryGetValue(_apiSettings.ApiKeyHeaderName, out var apiKey)
                || !ValidateApiKey(apiKey))
            {
                var details = new ProblemDetails
                {
                    Status = StatusCodes.Status401Unauthorized,
                    Title = "Unauthorized",
                    Type = "https://tools.ietf.org/html/rfc7235#section-3.1"
                };

                context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                context.Response.ContentType = "application/problem+json";
                await context.Response.WriteAsJsonAsync(details);
                return;
            }

            await _next(context);
        }

        /// <summary>
        /// Api key validation using test key in the appsettings
        /// </summary>
        private bool ValidateApiKey(string apiKey)
        {
            return apiKey == _apiSettings.ApiKey;
        }
    }


}
