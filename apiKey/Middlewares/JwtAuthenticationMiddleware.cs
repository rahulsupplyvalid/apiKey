using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Text;

namespace apiKey.Middlewares
{
    public class JwtAuthenticationMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly IConfiguration _configuration;

        public JwtAuthenticationMiddleware(RequestDelegate next, IConfiguration configuration)
        {
            _next = next;
            _configuration = configuration;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            // Extract the JWT token from the Authorization header
            var token = context.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();

            if (!string.IsNullOrEmpty(token))
            {
                try
                {
                    // Validate the token
                    var tokenHandler = new JwtSecurityTokenHandler();
                    var key = Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]);
                    var validationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidateLifetime = true,
                        ValidateIssuerSigningKey = true,
                        ValidIssuer = _configuration["Jwt:Issuer"],
                        ValidAudience = _configuration["Jwt:Audience"],
                        IssuerSigningKey = new SymmetricSecurityKey(key)
                    };

                    var principal = tokenHandler.ValidateToken(token, validationParameters, out var validatedToken);

                    // Set the principal (user) to the HttpContext
                    context.User = principal;
                }
                catch (SecurityTokenException)
                {
                    context.Response.StatusCode = 401; // Unauthorized
                    await context.Response.WriteAsync("Invalid JWT token.");
                    return;
                }
                catch (Exception)
                {
                    context.Response.StatusCode = 500; // Internal Server Error
                    await context.Response.WriteAsync("An error occurred while processing the JWT token.");
                    return;
                }
            }

            // Proceed with the next middleware
            await _next(context);
        }
    }
}
