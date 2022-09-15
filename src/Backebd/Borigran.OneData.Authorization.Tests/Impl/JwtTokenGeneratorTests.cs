using AutoFixture.NUnit3;
using Borigran.OneData.Authorization.Domain.Entities;
using Borigran.OneData.Authorization.Impl;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Borigran.OneData.Authorization.Tests.Impl
{
    public class JwtTokenGeneratorTests
    {

        [Test, AutoData]
        public void GenerateToken_Test(User user, [Frozen] AuthOptions authoptions, JwtTokenGenerator sut)
        {
            authoptions.AuthTokenExpired = 5;
            var token = sut.GenerateAccessTokenForUser(user);

            Assert.NotNull(token);
        }

        [Test, AutoData]
        public void Validate_ValidToken_Test(User user, [Frozen] AuthOptions authoptions, JwtTokenGenerator sut)
        {
            authoptions.AuthTokenExpired = 5;
            var token = sut.GenerateAccessTokenForUser(user);

            ClaimsPrincipal principal = sut.GetPrincipalFromToken(token);

            Assert.NotNull(principal);
            Assert.That(principal.FindFirst(ClaimTypes.Name)?.Value, Is.EqualTo(user.PhoneNumber));
            Assert.That(principal.FindFirst(ClaimTypes.SerialNumber)?.Value, Is.EqualTo(user.Id.ToString()));
        }

        [Test, AutoData]
        public void Validate_ExpiredToken_Test(User user, [Frozen] AuthOptions authoptions, JwtTokenGenerator sut)
        {
            authoptions.AuthTokenExpired = -5;
            var token = sut.GenerateAccessTokenForUser(user);

            ClaimsPrincipal principal = sut.GetPrincipalFromToken(token, true);

            Assert.NotNull(principal);
            Assert.That(principal.FindFirst(ClaimTypes.Name)?.Value, Is.EqualTo(user.PhoneNumber));
            Assert.That(principal.FindFirst(ClaimTypes.SerialNumber)?.Value, Is.EqualTo(user.Id.ToString()));
        }

        [Test, AutoData]
        public void ValidateToken_InvalidIssuer_Test(User user, [Frozen] AuthOptions authoptions, JwtTokenGenerator sut)
        {
            authoptions.AuthTokenExpired = 5;
            var token = sut.GenerateAccessTokenForUser(user);

            authoptions.Issuer = "newIssuer";

            Assert.Throws<SecurityTokenInvalidIssuerException>(() =>
            {
                sut.GetPrincipalFromToken(token);
            });
        }

        [Test, AutoData]
        public void ValidateToken_Expired_Test(User user, [Frozen] AuthOptions authoptions, JwtTokenGenerator sut)
        {
            authoptions.AuthTokenExpired = -5;
            var token = sut.GenerateAccessTokenForUser(user);

            authoptions.Issuer = "newIssuer";

            Assert.Throws<SecurityTokenExpiredException>(() =>
            {
                sut.GetPrincipalFromToken(token);
            });
        }
    }
}
