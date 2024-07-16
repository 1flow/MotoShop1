using Microsoft.AspNetCore.Mvc;
using MotoShop.Api.Swagger;
using MotoShop.Domain.Dto.Order;
using MotoShop.Domain.Entity;
using MotoShop.Domain.Interfaces.Services;
using MotoShop.Domain.Result;

namespace MotoShop.Api.Controllers
{

    [ApiController]
    [HttpResponses("Specifies HTTP responses for the methods.")]
    [Route("api/[controller]")]
    public class OrderController : ControllerBase
    {
        private readonly IOrderService _orderService;

        public OrderController(IOrderService orderService)
        {
            _orderService = orderService;
        }
        /// <summary>
        /// Добавление мотоцикла
        /// </summary>
        /// <remarks>
        /// 
        /// user - идентифиактор пользователя, который делает заказ
        /// motorcycleList - идентификаторы мотоциклов, которые входят в заказ    
        ///     
        /// 
        /// </remarks>
        /// <param name="dto">dto создания мотоцикла</param>        
        /// <response code="200">Если мотоцикл успешно добавлен</response>
        /// <returns></returns>
        [HttpPost]        
        public async Task<ActionResult<BaseResult<Order>>> CreateOrder([FromBody] CreateOrderDto dto)
        {
            var response = await _orderService.CreateOrderAsync(dto);
            if (response.IsSucces)
            {
                return Ok(response);
            }
            return BadRequest(response);
        }

        /// <summary>
        /// Получение заказов пользователя
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
        /// <param name="userId">Идентифиактор пользователя, заказы которого нужно получить</param>
        /// <response code="200">Если заказы были найдены</response>
        /// <returns></returns>
        [HttpGet("{userId}")]        
        public async Task<ActionResult<BaseResult<OrderDto>>> GetUsersOrders(long userId)
        {
            var response = await _orderService.GetUsersOrdersAsync(userId);
            if (response.IsSucces)
            {
                return Ok(response);
            }
            return BadRequest(response);
        }

        /// <summary>
        /// Удаление заказа с указанием идентификатора
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
        /// <param name="id">Идентифиактор заказа</param>
        /// <response code="200">Если заказ был удален</response>
        /// <returns></returns>
        [HttpDelete("{id}")]        
        public async Task<ActionResult<BaseResult<User>>> Delete(long id)
        {
            var response = await _orderService.DeleteOrderAsync(id);
            if (response.IsSucces)
            {
                return Ok(response);
            }
            return BadRequest(response);
        }

    }
}
