using MotoShop.Domain.Dto.Motorcycle;
using MotoShop.Domain.Result;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MotoShop.Domain.Interfaces.Services
{
    public interface IMotorcycleService
    {
        /// <summary>
        /// Получение всех мотоциклов
        /// </summary>
        /// <returns></returns>
        Task<CollectionResult<MotorcycleDto>> GetMotorcyclesAsync();

        /// <summary>
        /// получение мотоцикла по айди
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<BaseResult<MotorcycleDto>> GetMotorcycleByIdAsync(long id);

        /// <summary>
        /// Создание мотоцикла с базовыми параметрами
        /// </summary>
        /// <param name="report"></param>
        /// <returns></returns>
        Task<BaseResult<MotorcycleDto>> CreateMotorcycleAsync(CreateMotorcycleDto dto);

        /// <summary>
        /// Удаление мотоцикла по Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<BaseResult<MotorcycleDto>> DeleteMotorcycleAsync(long id);

        /// <summary>
        /// обновление данных мотоцикла
        /// </summary>
        /// <param name="updateReportDto"></param>
        /// <returns></returns>
        Task<BaseResult<MotorcycleDto>> UpdateMotorcycleAsync(UpdateMotorcycleDto dto);
        
    }
}
