﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MotoShop.Domain.Dto.User
{
    public record UpdateUserDto(long Id, string NewPassword);
    
}
