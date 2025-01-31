using api_key_Authorize.Models;

namespace api_key_Authorize.Repository
{
    public interface IDbRepository
    {
        //Task<List<string>> GetKeys();
        Task<bool> ValidateApiKey(HttpContext httpContext);
    }
}
