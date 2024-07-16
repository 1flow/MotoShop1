using FonTech.Domain.Dto.User;
using MotoShop.Domain.Dto.Motorcycle;
using MotoShop.Domain.Dto.User;



namespace MotoShop.Domain.Dto.Order
{
    public record OrderDto(long Id, long UserId, List<MotorcycleDto> MotorcycleList, string DeliveryAddress, DateTime OrderDate);
    
}
