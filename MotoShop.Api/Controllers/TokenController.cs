using Microsoft.AspNetCore.Mvc;
using MotoShop.Domain.Dto;
using MotoShop.Domain.Interfaces.Services;
using MotoShop.Domain.Result;

namespace MotoShop.Api.Controllers
{
    [ApiController]
    public class TokenController : Controller
    {
        private readonly ITokenService _tokenService;

        public TokenController(ITokenService tokenService)
        {
            _tokenService = tokenService;
        }

        [HttpPost]
        [Route("refresh")]
        public async Task<ActionResult<BaseResult<TokenDto>>> RefreshToken([FromBody] TokenDto tokenDto)
        {
            var response = await _tokenService.RefreshToken(tokenDto);
            if (response.IsSucces)
            {
                return Ok(response);
            }
            return BadRequest(response);
        }
    }
}
