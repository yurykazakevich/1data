using Borigran.OneData.Authorization.Domain.Entities;
using System.Security.Claims;

namespace Borigran.OneData.Authorization
{
    public interface ITokenGenerator
    {
        string GenerateAccessTokenForUser(User user, DateTime now);

        ClaimsPrincipal GetPrincipalFromToken(string token, bool isExpired = false);
    }
}
