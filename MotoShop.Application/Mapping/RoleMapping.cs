using AutoMapper;
using MotoShop.Domain.Dto.Motorcycle;
using MotoShop.Domain.Dto.Role;
using MotoShop.Domain.Entity;

namespace MotoShop.Application.Mapping
{
    public class RoleMapping : Profile
    {
        public RoleMapping()
        {
                this.CreateMap<Role, RoleDto>()
                .ReverseMap();

                
        }
    }
}
