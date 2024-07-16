using MotoShop.Domain.Dto.Role;
using MotoShop.Domain.Dto.UserRole;
using MotoShop.Domain.Entity;
using MotoShop.Domain.Result;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MotoShop.Domain.Interfaces.Services
{   /// <summary>
    /// Сервис для управления ролями
    /// </summary>
    public interface IRoleService 
    {
        /// <summary>
        /// Создание роли
        /// </summary>
        /// <param name="role"></param>
        /// <returns></returns>
        Task<BaseResult<RoleDto>> CreateRoleAsync(CreateRoleDto roleDto);

        /// <summary>
        /// Обноление роли
        /// </summary>
        /// <param name="roleDto"></param>
        /// <returns></returns>
        Task<BaseResult<RoleDto>> UpdateRoleAsync(RoleDto roleDto);

        /// <summary>
        /// Удаление роли
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<BaseResult<RoleDto>> DeleteRoleAsync(long id);

        /// <summary>
        /// Присвоение роли пользователю
        /// </summary>
        /// <param name="roleDto"></param>
        /// <returns></returns>
        Task<BaseResult<UserRoleDto>> AddRoleForUserAsync(UserRoleDto roleDto);

        /// <summary>
        /// Удаление роли у пользователя
        /// </summary>
        /// <param name="roleDto"></param>
        /// <returns></returns>
        Task<BaseResult<UserRoleDto>> DeleteRoleForUserAsync(DeleteUserRoleDto roleDto);

        /// <summary>
        /// Обновление роли у пользователя
        /// </summary>
        /// <param name="roleDto"></param>
        /// <returns></returns>
        Task<BaseResult<UserRoleDto>> UpdateRoleForUserAsync(UpdateUserRoleDto roleDto);



    }
}
