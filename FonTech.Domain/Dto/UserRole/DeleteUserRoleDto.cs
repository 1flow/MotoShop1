using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MotoShop.Domain.Dto.UserRole
{
    public class DeleteUserRoleDto
    {
        public string Login {  get; set; }

        public int RoleId { get; set; }
    }
}
