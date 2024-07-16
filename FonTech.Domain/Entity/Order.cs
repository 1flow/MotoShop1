using FonTech.Domain.Dto.User;
using MotoShop.Domain.Dto.Motorcycle;
using MotoShop.Domain.Dto.User;
using MotoShop.Domain.Entity;
using MotoShop.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MotoShop.Domain.Entity
{
    public class Order : IEntityId<long>
    {
        public long Id { get; set; }
        public string DeliveryAddress { get; set; }
        public List<Motorcycle> motorcycleList { get; set; } 
        public DateTime OrderDate { get; set; }
        public User User { get; set; }
        public long UserId { get; set; }
       

    }
}
