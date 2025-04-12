using EcommerceData.Models;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace EcommerceService.JwtTokenHelper
{
    public class JwtTokenHelper
    {
        public static (string accessToken, string refreshToken) GenerateTokens(SystemUser user, string secretKey, int accessTokenExpiryMinutes, string issuer, string audience)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var keyBytes = Encoding.UTF8.GetBytes(secretKey);
            var key = new SymmetricSecurityKey(keyBytes);

            // Prepare claims for the access token
            var claims = new Dictionary<string, object>
            {
                { ClaimTypes.NameIdentifier, user.Id.ToString() },
                { ClaimTypes.Email, user.Email ?? string.Empty },
                { "FirstName", user.FirstName ?? string.Empty },
                { "LastName", user.LastName ?? string.Empty },
                { ClaimTypes.Role, user.Role.ToString() }
            };

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Claims = claims,
                Expires = DateTime.Now.AddMinutes(accessTokenExpiryMinutes),
                Issuer = issuer,
                Audience = audience,
                SigningCredentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256)
            };

            var accessToken = tokenHandler.CreateToken(tokenDescriptor);
            string accessTokenString = tokenHandler.WriteToken(accessToken);

            // Generate a Refresh Token (randomly generated string)
            string refreshToken = Convert.ToBase64String(RandomNumberGenerator.GetBytes(64));

            // Return both tokens
            return (accessTokenString, refreshToken);
        }
    }
}
