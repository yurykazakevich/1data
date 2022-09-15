using Borigran.OneData.Authorization.Domain.Entities;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;

namespace Borigran.OneData.Authorization
{
    public interface ITokenGenerator
    {
        string GenerateAccessTokenForUser(User user);

        ClaimsPrincipal GetPrincipalFromToken(string token, bool isExpired = false);
    }
}
