using FluentValidation;
using MotoShop.Domain.Dto.Motorcycle;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MotoShop.Application.Validations.FluentValidations
{
    public class UpdateMotorcycleValidator : AbstractValidator<UpdateMotorcycleDto>
    {
        public UpdateMotorcycleValidator()
        {
            RuleFor(x => x.Id).NotEmpty();
            RuleFor(x => x.Decription).MaximumLength(100).NotEmpty();

        }
    }
}
