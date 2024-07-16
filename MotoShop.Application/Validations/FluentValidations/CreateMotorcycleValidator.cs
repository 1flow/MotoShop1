using FluentValidation;
using MotoShop.Domain.Dto.Motorcycle;
using MotoShop.Domain.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MotoShop.Application.Validations.FluentValidations
{
    public class CreateMotorcycleValidator : AbstractValidator<CreateMotorcycleDto>
    {
        public CreateMotorcycleValidator()
        {
            RuleFor(x => x.MotorcycleModel).IsInEnum();
            RuleFor(x => x.Name).NotEmpty().MaximumLength(45);
            RuleFor(x => x.MotorcycleType).IsInEnum();
            RuleFor(x => x.Description).NotEmpty().MaximumLength(2000);
            RuleFor(x => x.Capacity).LessThan(2500).WithMessage("Указан невозможно большой объем двигателя");
            
        }
    }
}
