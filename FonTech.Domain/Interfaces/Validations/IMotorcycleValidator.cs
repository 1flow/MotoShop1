using MotoShop.Domain.Entity;
using MotoShop.Domain.Result;

namespace MotoShop.Domain.Interfaces.Validations
{
    /// <summary>
    /// Проверяется наличие такого мотоцикла в базе,если есть - доабвить точно такой же не выйдет
    /// </summary>
    public interface IMotorcycleValidator : IBaseValidator<Motorcycle>
    {
        BaseResult CreateMotorcycleValidator(Motorcycle motorcycle);
    }
}
