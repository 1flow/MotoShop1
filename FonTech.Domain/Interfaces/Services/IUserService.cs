using FonTech.Domain.Dto.User;
using MotoShop.Domain.Dto.User;
using MotoShop.Domain.Result;


namespace MotoShop.Domain.Interfaces.Services
{
    public interface IUserService
    {
        /// <summary>
        /// Получение всех мотоциклов
        /// </summary>
        /// <returns></returns>
        Task<CollectionResult<UserDto>> GetUsersAsync();

        /// <summary>
        /// получение мотоцикла по айди
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<BaseResult<UserDto>> GetUserByIdAsync(long id);

        /// <summary>
        /// Создание мотоцикла с базовыми параметрами
        /// </summary>
        /// <param name="report"></param>
        /// <returns></returns>
        Task<BaseResult<UserDto>> CreateUserAsync(RegisterUserDto dto);

        /// <summary>
        /// Удаение мотоцикла по Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<BaseResult<UserDto>> DeleteUserAsync(long id);

        /// <summary>
        /// обновление данных мотоцикла
        /// </summary>
        /// <param name="updateReportDto"></param>
        /// <returns></returns>
        Task<BaseResult<UserDto>> UpdateUserAsync(UpdateUserDto dto);
        
    }
}
