using AutoMapper;
using MotoShop.Domain.Dto.Motorcycle;
using MotoShop.Domain.Entity;

namespace MotoShop.Application.Mapping
{
    public class MotorcycleMapping : Profile
    {
        public MotorcycleMapping()
        {
                this.CreateMap<Motorcycle, MotorcycleDto>()
                .ReverseMap();

                
        }
    }
}
