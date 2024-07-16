using MotoShop.Application.Resources;
using MotoShop.Domain.Entity;
using MotoShop.Domain.Enum;
using MotoShop.Domain.Interfaces.Validations;
using MotoShop.Domain.Result;

namespace MotoShop.Application.Validations
{
    public class MotorcycleValidator : IMotorcycleValidator
    {
        public BaseResult CreateMotorcycleValidator(Motorcycle motorcycle)
        {
            if (motorcycle is not null)
            {
                return new BaseResult()
                {
                    ErrorMessage = ErrorMessage.MotorcycleAlreadyExists,
                    ErrorCode = (int?)ErrorCodes.MotorcycleAlreadyExists
                };
            }
            return new BaseResult();
        }

        public BaseResult ValidteOnNull(Motorcycle motorcycle)
        {
            if (motorcycle == null)
            {
                return new BaseResult()
                {
                    ErrorMessage = ErrorMessage.MotorcycleNotFound,
                    ErrorCode = (int?)ErrorCodes.MotorcycleNotFound
                };
            }
            return new BaseResult() { };

        }
    }
}
