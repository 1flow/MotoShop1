using AutoMapper;
using MotoShop.Domain.Dto.Motorcycle;
using MotoShop.Domain.Dto.Order;
using MotoShop.Domain.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MotoShop.Application.Mapping
{
    public class OrderMapping : Profile
    {
        public OrderMapping()
        {
            this.CreateMap<Order, OrderDto>()
            .ReverseMap();


        }
    }
}
