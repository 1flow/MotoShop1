using AutoMapper;
using FonTech.Domain.Dto.User;
using MotoShop.Domain.Dto.Motorcycle;
using MotoShop.Domain.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MotoShop.Application.Mapping
{
    public class UserMapping : Profile
    {
        public UserMapping()
        {
            this.CreateMap<User, UserDto>()
            .ReverseMap();


        }
    }
}
