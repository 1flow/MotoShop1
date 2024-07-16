using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MotoShop.Api.Swagger;
using MotoShop.Domain.Dto.Motorcycle;
using MotoShop.Domain.Entity;
using MotoShop.Domain.Interfaces.Services;
using MotoShop.Domain.Result;

namespace MotoShop.Api.Controllers
{
    [Authorize]
    [ApiController]
    [HttpResponses("Specifies HTTP responses for the methods.")]
    [Route("api/[controller]")]
    public class MotorcycleController : ControllerBase
    {
        private readonly IMotorcycleService _motorcycleService;

        public MotorcycleController(IMotorcycleService motorcycleService)
        {
            _motorcycleService = motorcycleService;
        }

        /// <summary>
        /// Получение Мотоцикла по айди
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
        /// <param name="id">Мотоцикл</param>
        /// <response code="200">Если мотоцикл был найден</response>
        /// <response code="400">Если мотоцикл не был найден</response>
        /// <returns>Мотоцикл с указанным id</returns>
        [HttpGet("{id}")]        
        public async Task<ActionResult<BaseResult<MotorcycleDto>>> GetMotorcycle(long id)
        {
            var response = await _motorcycleService.GetMotorcycleByIdAsync(id);
            if (response.IsSucces)
            {
                return Ok(response);
            }
            return BadRequest(response);
        }

        /// <summary>
        /// Получение всех имеющихся мотоциклов
        /// </summary>             
        /// <response code="200">Если мотоцикл были найдены и возвращены</response>
        /// <response code="400">Если мотоцикл не были найдены</response>
        [HttpGet]        
        public async Task<ActionResult<BaseResult<MotorcycleDto>>> GetMotorcycles()
        {
            var response = await _motorcycleService.GetMotorcyclesAsync();
            if (response.IsSucces)
            {
                return Ok(response);
            }
            return BadRequest(response);
        }

        /// <summary>
        /// Удаление мотоцикла с указанным идентификатором
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
        /// <param name="id">Идентификатор мотоцикла</param>
        /// <response code="200">Если мотоцикл был удален</response>
        /// <response code="400">Если мотоцикл не удалось удалить</response>
        /// <returns></returns>
        [HttpDelete("{id}")]        
        public async Task<ActionResult<BaseResult<Motorcycle>>> Delete(long id)
        {
            var response = await _motorcycleService.DeleteMotorcycleAsync(id);
            if (response.IsSucces)
            {
                return Ok(response);
            }
            return BadRequest(response);
        }
        /// <summary>
        /// Добавление мотоцикла
        /// </summary>
        /// <remarks>
        /// Пример запроса:
        ///
        ///     POST 
        ///     {
        ///         
        ///        "name" : "Yamaha FZS600",
        ///        "description" : "Отличный дорожный мотоцикл с обьемом двигателя 600куб см, мощностью 95л.с.",
        ///        "capacity": "600",
        ///        "motorcycleModel": "0",
        ///        "motorcycleType": "5",
        ///        "price": "3000",
        ///        
        ///     }
        ///     motorcycleModel
        ///     Yamaha = 0,
        ///     Kawasaki = 1,
        ///     Suzuki = 2,
        ///     Honda = 3
        ///
        ///     MotorcycleType
        ///     Touring = 0,
        ///     Sport = 1,
        ///     Offroad = 2,
        ///     Cruiser = 3,
        ///     Sporttouring = 4,
        ///     Roadster = 5,
        /// 
        /// </remarks>
        /// <param name="dto">dto создания мотоцикла</param>        
        /// <response code="200">Если мотоцикл успешно добавлен</response>
        /// <returns></returns>
        [HttpPost]        
        public async Task<ActionResult<BaseResult<Motorcycle>>> Create([FromBody] CreateMotorcycleDto dto)
        {
            var response = await _motorcycleService.CreateMotorcycleAsync(dto);
            if (response.IsSucces)
            {
                return Ok(response);
            }
            return BadRequest(response);
        }
        /// <summary>
        /// Обновление данных мотоцикла с указанием оснонвых свойств
        /// </summary>
        /// <remarks>
        /// Пример запроса:
        ///
        ///     PUT 
        ///     {                               
        ///        "id": "1",
        ///        "description" : "Спортивный мотоцикл с разгоном до 100km/h за 2.9сек",
        ///        "price" : "7000"
        ///     }
        ///
        /// </remarks>
        /// <response code="200">Если данные мотоцикла успешно обновлены</response>
        /// <returns></returns>
        [HttpPut]        
        public async Task<ActionResult<BaseResult<Motorcycle>>> Update([FromBody] UpdateMotorcycleDto dto)
        {
            var response = await _motorcycleService.UpdateMotorcycleAsync(dto);
            if (response.IsSucces)
            {
                return Ok(response);
            }
            return BadRequest(response);
        }
    }
}
