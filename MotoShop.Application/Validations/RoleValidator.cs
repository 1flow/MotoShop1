using Microsoft.AspNetCore.Identity;
using MotoShop.Application.Resources;
using MotoShop.Domain.Entity;
using MotoShop.Domain.Enum;
using MotoShop.Domain.Interfaces.Validations;
using MotoShop.Domain.Result;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MotoShop.Application.Validations
{
    public class RoleValidator : IRoleValidator
    {
        public BaseResult ValidteOnNull(Role role)
        {
            if (role != null)
            {
                return new BaseResult()
                {
                    ErrorMessage = ErrorMessage.RoleAlreadyExists,
                    ErrorCode = (int?)ErrorCodes.RoleAlreadyExists
                };
            }
            return new BaseResult() { };


        }
    }
}
