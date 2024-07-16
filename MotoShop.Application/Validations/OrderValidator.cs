using MotoShop.Application.Resources;
using MotoShop.Domain.Entity;
using MotoShop.Domain.Enum;
using MotoShop.Domain.Interfaces.Validations;
using MotoShop.Domain.Result;

namespace MotoShop.Application.Validations
{
    public class OrderValidator : IOrderValidator
    {
        

        public BaseResult ValidteOnNull(Order order)
        {
            if (order == null)
            {
                return new BaseResult()
                {
                    ErrorMessage = ErrorMessage.OrderNotFound,
                    ErrorCode = (int?)ErrorCodes.OrderNotFound
                };
            }
            return new BaseResult() { };

        }
    }
}
