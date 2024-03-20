using Microsoft.AspNetCore.Mvc;
using System.Web.Mvc;
using IExceptionFilter = Microsoft.AspNetCore.Mvc.Filters.IExceptionFilter;

namespace CSI.IBTA.Administrator.Middleware
{
    public class ErrorHandlingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ErrorHandlingMiddleware> _logger;

        public ErrorHandlingMiddleware(RequestDelegate next, ILogger<ErrorHandlingMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An unexpected error occurred.");

                var statusCode = context.Response.StatusCode;
                if (statusCode == StatusCodes.Status500InternalServerError || statusCode == StatusCodes.Status404NotFound)
                {
                    context.Response.Redirect("/Error/Index");
                    return;
                }

                throw; // Re-throw the exception if it's not one of the handled status codes
            }
        }
    }
}
