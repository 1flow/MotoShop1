using MotoShop.Domain.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MotoShop.Domain.Dto.Motorcycle
{
    public record CreateMotorcycleDto(string Name, string Description, int Capacity, MotorcycleModel MotorcycleModel, MotorcycleType MotorcycleType, decimal Price);
}
