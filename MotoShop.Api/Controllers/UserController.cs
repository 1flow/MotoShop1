using Asp.Versioning;
using FonTech.Domain.Dto.User;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MotoShop.Api.Swagger;
using MotoShop.Domain.Dto.Motorcycle;
using MotoShop.Domain.Dto.User;
using MotoShop.Domain.Entity;
using MotoShop.Domain.Interfaces.Services;
using MotoShop.Domain.Result;

namespace MotoShop.Api.Controllers
{
    [Authorize]
    [ApiController]
    [ApiVersion("1.0")]
    [HttpResponses("Specifies HTTP responses for the methods.")]
    [Route("api/v{version:apiVersion}/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        /// <summary>
        /// Получение пользователя по айли
        /// </summary>
        /// <remarks>
        /// Пример запроса:
        ///
        ///     GET 
        ///     {                               
        ///        "id": "1"
        ///     }
        ///
        /// </remarks>
        /// <param name="id">Пользователь</param>
        /// <response code="200">Если пользователь был найден</response>
        /// <returns></returns>
        [HttpGet("{id}")]        
        public async Task<ActionResult<BaseResult<MotorcycleDto>>> GetUser(long id)
        {
            var response = await _userService.GetUserByIdAsync(id);
            if (response.IsSucces)
            {
                return Ok(response);
            }
            return BadRequest(response);
        }

        [HttpGet]        
        public async Task<ActionResult<BaseResult<MotorcycleDto>>> GetUsers()
        {
            var response = await _userService.GetUsersAsync();
            if (response.IsSucces)
            {
                return Ok(response);
            }
            return BadRequest(response);
        }

        /// <summary>
        /// Удаление пользователя с указанием идентификатора
        /// </summary>
        /// <remarks>
        /// Пример запроса:
        ///
        ///     GET 
        ///     {                               
        ///        "id": "1"
        ///     }
        ///
        /// </remarks>
        /// <param name="id">Идентификатор пользователя</param>
        /// <response code="200">Если пользователь был удален</response>
        /// <returns></returns>
        [HttpDelete("{id}")]               
        public async Task<ActionResult<BaseResult<User>>> Delete(long id)
        {
            var response = await _userService.DeleteUserAsync(id);
            if (response.IsSucces)
            {
                return Ok(response);
            }
            return BadRequest(response);
        }
        /// <summary>
        /// Создание пользователя
        /// </summary>
        /// <remarks>
        /// Пример запроса:
        ///
        ///     POST 
        ///     {
        ///         
        ///        "login" : "User12345",
        ///        "password" : "root123",
        ///        "passwordConfirm": "root123"
        ///     }
        ///
        /// </remarks>
        /// <param name="dto">Пользователь</param>
        /// <response code="200">Если пользователь был зарегистрирован</response>
        /// <returns></returns>
        [HttpPost]   
        public async Task<ActionResult<BaseResult<User>>> Create([FromBody] RegisterUserDto dto)
        {
            var response = await _userService.CreateUserAsync(dto);
            if (response.IsSucces)
            {
                return Ok(response);
            }
            return BadRequest(response);
        }
        /// <summary>
        /// Обновление пароля пользователя
        /// </summary>        
        /// <response code="200">Если пароль был обновлен</response>
        /// <returns></returns>
        [HttpPut]
        public async Task<ActionResult<BaseResult<User>>> Update([FromBody] UpdateUserDto dto)
        {
            var response = await _userService.UpdateUserAsync(dto);
            if (response.IsSucces)
            {
                return Ok(response);
            }
            return BadRequest(response);
        }
    }
}
