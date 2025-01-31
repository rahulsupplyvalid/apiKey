using Microsoft.AspNetCore.Authorization;

namespace apiKey.Middlewares
{
    public class ApiKeyRequirement : IAuthorizationRequirement
    {
        public ApiKeyRequirement() { }
    }
}
