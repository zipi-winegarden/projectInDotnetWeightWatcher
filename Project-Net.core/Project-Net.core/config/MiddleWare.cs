using Newtonsoft.Json;
using Microsoft.AspNetCore.Http.Extensions;
using System.Text.Json.Serialization;

namespace Project_Net.core.config
{
    public class MiddleWare
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<MiddleWare> _logger;


        public MiddleWare(RequestDelegate next, ILogger<MiddleWare> logger)
        {
            _next = next;   
            _logger = logger;
        }
        public async Task Invoke(HttpContext context /* other dependencies */)
        {
            // Log the start of request
            _logger.LogInformation($"Request received at: {DateTime.Now}");
            try
            {
                await _next(context);
                // Log success if no exception occurred
                _logger.LogInformation("Request processed successfully.");
            }
            catch (Exception ex)
            {   
                await HandleExceptionAsync(context, ex);

            }
        }
        private Task HandleExceptionAsync(HttpContext context, Exception ex)
        {

            // Log the exception
            _logger.LogError(ex, "An unhandled exception has occurred.");

            // Create response
            var response = new { error = ex.Message };

            // Serialize response
            var jsonResponse = JsonConvert.SerializeObject(response);

            // Set response status code
            context.Response.StatusCode = StatusCodes.Status500InternalServerError;


            _logger.LogInformation("Exception handled successfully.");
            // Write response to the client
            return   context.Response.WriteAsync(jsonResponse);

            
        }

    }
}
