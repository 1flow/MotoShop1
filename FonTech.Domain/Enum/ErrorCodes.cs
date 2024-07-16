using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MotoShop.Domain.Enum
{
    public enum ErrorCodes
    {
        // 0-9 MotorcycleError
        MotorcyclesNotFound = 0,
        MotorcycleNotFound = 1,
        MotorcycleAlreadyExists = 2,

        InternalServerError = 10,

        //11-20 UserError
        UserNotFound = 11,
        UserAlreadyExists = 12,
        PasswordNotEqualsPasswordConfirm = 13,
        PasswordIsWrong = 14,
        UserUnauthorizedAccess = 15,
        UserWithThatRoleAlreadyExists = 16,

        //21-30 OrderError
        OrderNotFound = 22,
        OrdersNotFound = 23,

        //31-40 RoleError
        RoleNotFound = 31,
        RoleAlreadyExists = 32,
        




    }
}
