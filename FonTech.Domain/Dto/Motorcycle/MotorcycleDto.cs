using MotoShop.Domain.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MotoShop.Domain.Dto.Motorcycle
{
    public record MotorcycleDto(long Id, MotorcycleModel MotorcycleModel, string Name);
}
