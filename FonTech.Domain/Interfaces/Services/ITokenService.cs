using MotoShop.Domain.Dto;
using MotoShop.Domain.Result;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace MotoShop.Domain.Interfaces.Services
{
    public interface ITokenService
    {
        public string GenerateAccessToken(IEnumerable<Claim> claims);

        public string GenerateRefreshToken();

        public ClaimsPrincipal GetPrincipalFromExpiredToken(string accessToken);

        Task<BaseResult<TokenDto>> RefreshToken(TokenDto tokenDto);
        
    }
}
