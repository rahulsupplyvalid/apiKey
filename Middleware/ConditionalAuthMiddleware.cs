using masterapi.Data;
using Microsoft.EntityFrameworkCore;

namespace masterapi.Middleware
{
    public class ConditionalAuthMiddleware
    {
        private readonly RequestDelegate _next;

        public ConditionalAuthMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context, AuthMasterDbContext dbContext)
        {
            var headers = context.Request.Headers;

            // Check for JWT token
            if (headers.ContainsKey("Authorization"))
            {
                var token = headers["Authorization"].ToString().Replace("Bearer ", "");
                // Validate JWT token
                var jwtValid = ValidateJwtToken(token); // Implement JWT validation logic
                if (!jwtValid)
                {
                    context.Response.StatusCode = 401;
                    await context.Response.WriteAsync("Invalid JWT token.");
                    return;
                }
            }
            // Check for Secret Key
            else if (headers.ContainsKey("X-Api-Key"))
            {
                var secretKey = headers["X-Api-Key"].ToString();
                var project = await dbContext.Projects.FirstOrDefaultAsync(p => p.SecretKey == secretKey);

                if (project == null)
                {
                    context.Response.StatusCode = 401;
                    await context.Response.WriteAsync("Invalid API Key.");
                    return;
                }
            }
            else
            {
                context.Response.StatusCode = 401;
                await context.Response.WriteAsync("Authorization or API Key is required.");
                return;
            }

            // If validation passes, proceed to the next middleware
            await _next(context);
        }

        private bool ValidateJwtToken(string token)
        {
            // Implement JWT validation logic here
            // For example, use JwtSecurityTokenHandler to validate the token
            return true;
        }
    }
}
