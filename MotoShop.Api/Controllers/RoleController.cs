using Microsoft.AspNetCore.Mvc;
using MotoShop.Api.Swagger;
using MotoShop.Application.Services;
using MotoShop.Domain.Dto.Role;
using MotoShop.Domain.Dto.UserRole;
using MotoShop.Domain.Entity;
using MotoShop.Domain.Interfaces.Services;
using MotoShop.Domain.Result;
using System.Net.Mime;
using System.Runtime.CompilerServices;

namespace MotoShop.Api.Controllers
{
    [HttpResponses("Specifies HTTP responses for the methods.")]
    [Consumes(MediaTypeNames.Application.Json)]
    [ApiController]
    [Route("api/[controller]")]
    public class RoleController : ControllerBase
    {
        private readonly IRoleService _roleService;

        public RoleController(IRoleService roleService)
        {
            _roleService = roleService;
        }

        /// <summary>
        /// Создание роли
        /// </summary>
        /// <remarks>
        /// Пример запроса:
        ///
        ///     POST 
        ///     {
        ///        "Name" : "Moderator",
        ///        "Id" : "2",
        ///     }
        ///
        /// </remarks>
        /// <param name="roleDto">Роль</param>
        /// <response code="200">Если роль была создана</response>
        /// <returns></returns>
        [HttpPost]
        public async Task<ActionResult<BaseResult<RoleDto>>> Create([FromBody] CreateRoleDto roleDto)
        {
            var response = await _roleService.CreateRoleAsync(roleDto);
            if (response.IsSucces)
            {
                return Ok(response);
            }
            else return BadRequest(response); 
        }
        /// <summary>
        /// Удаление роли с указанием идентификатора
        /// </summary>
        /// <remarks>
        /// Пример запроса:
        ///
        ///     Delete 
        ///     {
        ///        "Id" : "2",
        ///     }
        ///
        /// </remarks>
        /// <param name="id">Роль</param>
        /// <response code="200">Если роль была удалена</response>
        /// <returns></returns>
        [HttpDelete("id")]
        public async Task<ActionResult<BaseResult<RoleDto>>> Delete(long id)
        {
            var response = await _roleService.DeleteRoleAsync(id);
            if (response.IsSucces)
            {
                return Ok(response);
            }
            else return BadRequest(response);
        }

        /// <summary>
        /// Обновление роли
        /// </summary>
        /// <remarks>
        /// Пример запроса:
        ///
        ///     POST 
        ///     {
        ///        "Name" : "Admin",
        ///        "Id" : "1",
        ///     }
        ///
        /// </remarks>
        /// <param name="roleDto">Роль</param>
        /// <response code="200">Если роль была обновлена</response>
        /// <returns></returns>
        [HttpPut]
        public async Task<ActionResult<BaseResult<RoleDto>>> Update([FromBody]RoleDto roleDto)
        {           
            var response = await _roleService.UpdateRoleAsync(roleDto);

            if (response.IsSucces)
            {
                return Ok(response);
            }
            else return BadRequest(response);
        }

        /// <summary>
        /// Добавление роли пользователю
        /// </summary>
        /// <remarks>
        /// Пример запроса:
        ///
        ///     POST 
        ///     {
        ///        "Login" : "User123",
        ///        "RoleName" : "Admin",
        ///     }
        ///
        /// </remarks>
        /// <param name="userRoleDto">Роль</param>
        /// <response code="200">Если роль была присвоена</response>
        /// <returns></returns>
        [HttpPost("addRole")]
        public async Task<ActionResult<BaseResult<RoleDto>>> AddRoleForUser([FromBody] UserRoleDto userRoleDto)
        {
            var response = await _roleService.AddRoleForUserAsync(userRoleDto);
            if (response.IsSucces)
            {
                return Ok(response);
            }
            else return BadRequest(response);
        }

        /// <summary>
        /// Удаление роли у пользователя
        /// </summary>
        /// <remarks>
        /// Пример запроса:
        ///
        ///     POST 
        ///     {
        ///        "Login" : "User123",
        ///        "RoleId" : "2",
        ///     }
        ///
        /// </remarks>
        /// <param name="userRoleDto">Роль</param>
        /// <response code="200">Если роль была удалена</response>
        /// <returns></returns>
        [HttpDelete]
        public async Task<ActionResult<BaseResult<RoleDto>>> DeleteRoleForUser([FromBody] DeleteUserRoleDto userRoleDto)
        {
            var response = await _roleService.DeleteRoleForUserAsync(userRoleDto);
            if (response.IsSucces)
            {
                return Ok(response);
            }
            else return BadRequest(response);
        }

        /// <summary>
        /// Обновление роли у пользователя
        /// </summary>
        /// <remarks>
        /// Пример запроса:
        ///
        ///     POST 
        ///     {
        ///        "Login" : "User123",
        ///        "OldRoleId" : "1",
        ///        "NewRoleId" : "2",
        ///     }
        ///
        /// </remarks>
        /// <param name="userRoleDto">Роль</param>
        /// <response code="200">Если роль была обновлена</response>
        /// <returns></returns>
        [HttpPut("update-user-role")]
        public async Task<ActionResult<BaseResult<RoleDto>>> UpdateRoleForUser([FromBody] UpdateUserRoleDto userRoleDto)
        {
            var response = await _roleService.UpdateRoleForUserAsync(userRoleDto);
            if (response.IsSucces)
            {
                return Ok(response);
            }
            else return BadRequest(response);
        }
    }
}
