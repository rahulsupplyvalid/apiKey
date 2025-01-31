using System.IdentityModel.Tokens.Jwt;
using System.Text;
using api_key_Authorize.Repository;
using masterapi.Repositories.TokenRepository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace apiKey.Middlewares
{
    public class ApiKeyOrJwtHandler : AuthorizationHandler<ApiKeyOrJwtRequirement>
    {
        private readonly IDbRepository _dbRepository;
        private readonly ITokenRepository _tokenRepository;
        private readonly IConfiguration _configuration;

        public ApiKeyOrJwtHandler(IDbRepository dbRepository, ITokenRepository tokenRepository, IConfiguration configuration)
        {
            _dbRepository = dbRepository;
            _tokenRepository = tokenRepository;
            _configuration = configuration;
        }

        protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context, ApiKeyOrJwtRequirement requirement)
        {
            var httpContext = (context.Resource as HttpContext);

            if (httpContext == null)
            {
                context.Fail();
                return;
            }

            // Check if the request contains a valid API key
            bool isApiKeyValid = await _dbRepository.ValidateApiKey(httpContext);

            // Check if the request contains a valid JWT token
            //var jwtToken = httpContext.Request.Headers["Authorization"].ToString().Replace("Bearer ", "");
            var token = httpContext.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();

            //Console.WriteLine(token);

            bool isJwtValid = !string.IsNullOrEmpty(token);

            if (isApiKeyValid && isJwtValid)
            {
                context.Succeed(requirement); // Authorization succeeded
            }
            else
            {
                context.Fail(); // Authorization failed
            }
        }
    }
}
