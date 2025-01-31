using Microsoft.AspNetCore.Identity;

namespace masterapi.Repositories.TokenRepository
{
    public interface ITokenRepository
    {
        string CreateJWTToken(IdentityUser user, List<string> roles);
    }
}