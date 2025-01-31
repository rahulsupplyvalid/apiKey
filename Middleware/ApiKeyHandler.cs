using api_key_Authorize.Repository;
using Microsoft.AspNetCore.Authorization;

namespace apiKey.Middlewares
{
    public class ApiKeyHandler : AuthorizationHandler<ApiKeyRequirement>
    {
        private readonly IDbRepository _dbRepository;

        public ApiKeyHandler(IDbRepository dbRepository)
        {
            _dbRepository = dbRepository;
        }

        protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context, ApiKeyRequirement requirement)
        {
            var httpContext = (context.Resource as HttpContext);

            if (httpContext == null)
            {
                context.Fail();
                return;
            }

            bool isValidApiKey = await _dbRepository.ValidateApiKey(httpContext);

            if (isValidApiKey)
            {
                context.Succeed(requirement); // API key is valid
            }
            else
            {
                context.Fail(); // API key is invalid
            }
        }
    }

}
