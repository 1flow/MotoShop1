using MotoShop.Domain.Enum;
using MotoShop.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MotoShop.Domain.Entity
{
    public class Motorcycle : IEntityId<long>
    {
        public long Id { get; set; }
        public DateTime? CreatedAt { get; set; }        
        public string? Name { get; set; }
        public string? Description { get; set; }
        public int Capacity { get; set; }
        public decimal Price { get; set; }
        public MotorcycleModel MotorcycleModel { get; set; }
        public MotorcycleType MotorcycleType { get; set; }
        public List<Order> Orders { get; set; }
    }
}
