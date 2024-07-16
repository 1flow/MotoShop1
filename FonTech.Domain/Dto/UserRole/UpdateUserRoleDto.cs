using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MotoShop.Domain.Dto.UserRole
{
    public class UpdateUserRoleDto
    {
        public string Login { get; set; }

        public int OldRoleId { get; set; }

        public int NewRoleId { get; set; }
    }
}
