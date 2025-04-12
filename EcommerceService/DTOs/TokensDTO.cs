using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EcommerceService.DTOs
{
    public class TokensDTO
    {
        public string AccessToken { get; }
        public string RefreshToken { get; }

        public TokensDTO(string accessToken, string refreshToken)
        {
            AccessToken = accessToken;
            RefreshToken = refreshToken;
        }
    }
}
