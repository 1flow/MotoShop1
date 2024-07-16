using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MotoShop.Domain.Enum
{
    public enum MotorcycleType
    {
        [Display(Name = "Туристический")]
        Touring = 0,
        [Display(Name = "Спортбайк")]
        Sport = 1,
        [Display(Name = "Эндуро")]
        Offroad = 2,
        [Display(Name = "Круизер")]
        Cruiser = 3,
        [Display(Name = "Спорт-турист")]
        Sporttouring = 4,
        [Display(Name = "Дорожик")]
        Roadster = 5,
    }
}
