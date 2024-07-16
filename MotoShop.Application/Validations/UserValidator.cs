using MotoShop.Application.Resources;
using MotoShop.Domain.Entity;
using MotoShop.Domain.Enum;
using MotoShop.Domain.Interfaces.Validations;
using MotoShop.Domain.Result;

namespace MotoShop.Application.Validations
{
    public class UserValidator : IUserValidator
    {
        public BaseResult CreateUserValidator(User user)
        {
            if (user is not null)
            {
                return new BaseResult()
                {
                    ErrorMessage = ErrorMessage.UserAlreadyExists,
                    ErrorCode = (int?)ErrorCodes.UserAlreadyExists
                };
            }
            if (user is null)
            {
                return new BaseResult()
                {
                    ErrorMessage = ErrorMessage.UserNotFound,
                    ErrorCode = (int?)ErrorCodes.UserNotFound
                };
            }
            return new BaseResult();
        }

        public BaseResult ValidteOnNull(User user)
        {
            if (user == null)
            {
                return new BaseResult()
                {
                    ErrorMessage = ErrorMessage.UserNotFound,
                    ErrorCode = (int?)ErrorCodes.UserNotFound
                };
            }
            return new BaseResult() { };

        }
    }
}
