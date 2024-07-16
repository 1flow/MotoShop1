using FonTech.Domain.Dto.User;
using MotoShop.Domain.Dto.Motorcycle;
using MotoShop.Domain.Dto.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MotoShop.Domain.Dto.Order
{
    public record CreateOrderDto(UserIdDto User, List<MotorcycleIdDto> MotorcycleList, string DeliveryAddress, DateTime OrderDate);
}
