using Microsoft.AspNetCore.Authorization;

namespace apiKey.Middlewares
{
    public class ApiKeyOrJwtRequirement : IAuthorizationRequirement
    {
        // You can add properties or methods here if needed in the future for more complex scenarios.
    }

}
