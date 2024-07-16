using MotoShop.Domain.Entity;
using MotoShop.Domain.Result;

namespace MotoShop.Domain.Interfaces.Validations
{
    /// <summary>
    /// Проверяется наличие такого пользователя в базе,если есть - добавить точно такой же не выйдет
    /// </summary>
    public interface IUserValidator : IBaseValidator<User>
    {
        BaseResult CreateUserValidator(User motorcycle);
    }
}
