using CSI.IBTA.Consumer.Interfaces;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc;

namespace CSI.IBTA.Consumer.Filters
{
    public class AuthenticationFilter : IAsyncActionFilter
    {
        private readonly IJwtTokenService _jwtTokenService;

        public AuthenticationFilter(IJwtTokenService jwtTokenService)
        {
            _jwtTokenService = jwtTokenService;
        }

        public Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            string? token = _jwtTokenService.GetCachedToken();

            if (token == null)
            {
                context.Result = new RedirectToActionResult("Login", "Auth", null);
                return Task.CompletedTask;
            }

            bool isTokenValid = _jwtTokenService.IsTokenValid(token);

            if (!isTokenValid)
            {
                context.Result = new RedirectToActionResult("Logout", "Auth", null);
                return Task.CompletedTask;
            }

            return next();
        }
    }
}
