using shared.ConfigurationOptions;
using Microsoft.Extensions.Options;

namespace api.Middleware
{
    public class TokenAuthorizationMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ApiOptions _apiOptions;

        public TokenAuthorizationMiddleware(RequestDelegate next, IOptions<ApiOptions> apiOptions)
        {
            _next = next;
            _apiOptions = apiOptions.Value;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            if (!context.Request.Headers.TryGetValue("Authorization", out var authorizationHeader))
            {
                context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                return;
            }

            var token = authorizationHeader.FirstOrDefault()?.Split(" ").Last();
            if (token != _apiOptions.AccessToken)
            {
                context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                return;
            }

            await _next(context);
        }
    }
}
