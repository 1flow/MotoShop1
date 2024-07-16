using FonTech.Domain.Dto.User;
using MotoShop.Domain.Dto.Order;
using MotoShop.Domain.Dto.User;
using MotoShop.Domain.Result;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace MotoShop.Domain.Interfaces.Services
{
    public interface IOrderService
    {
        /// <summary>
        /// Получение всех заказов пользователя
        /// </summary>
        /// <returns></returns>
        Task<CollectionResult<OrderDto>> GetUsersOrdersAsync(long id);

        /// <summary>
        /// получение заказа по айди
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<BaseResult<OrderDto>> GetOrderByIdAsync(long id);

        /// <summary>
        /// Создание заказа 
        /// </summary>
        /// <param name="order"></param>
        /// <returns></returns>
        Task<BaseResult<OrderDto>> CreateOrderAsync(CreateOrderDto dto);

        /// <summary>
        /// Удаение заказа по Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<BaseResult<OrderDto>> DeleteOrderAsync(long id);

        /// <summary>
        /// обновление данных заказа
        /// </summary>
        /// <param name="OrderDto"></param>
        /// <returns></returns>
        Task<BaseResult<OrderDto>> UpdateOrderAsync(OrderDto dto);

    }
}
