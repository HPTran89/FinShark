using Microsoft.AspNetCore.Diagnostics;
using System.Diagnostics;

namespace api.Utility
{
    public class GlobalExceptionHandler : IExceptionHandler
    {
        private readonly ILogger<GlobalExceptionHandler> _logger;

        public GlobalExceptionHandler(ILogger<GlobalExceptionHandler> logger)
        {
            _logger = logger;
        }

        public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
        {
            var traceId = Activity.Current?.Id ?? httpContext.TraceIdentifier;

            _logger.LogError(exception, "Could not process request from machine {MachineName}. TraceId: {TraceI}", Environment.MachineName, traceId);

            var (statusCode, title) = MapException(exception);
            await Results.Problem(
                title: title,
                statusCode: statusCode,
                extensions: new Dictionary<string, object?>
                {
                    {"traceId", traceId},
                }
            ).ExecuteAsync(httpContext);

            // return true will make the pipeline stop here to use this middleware to handle exception
            return true;
        }

        private static (int statusCode, string title) MapException(Exception exception)
        {
            return exception switch
            {
                ArgumentException => (StatusCodes.Status400BadRequest, exception.Message),
                _ => (StatusCodes.Status500InternalServerError, "we are working on it"),
            };
        }
    }
}
 