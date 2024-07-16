using Microsoft.EntityFrameworkCore.Storage;
using MotoShop.Domain.Entity;
using MotoShop.Domain.Interfaces.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MotoShop.Domain.Interfaces.Databases
{
    public interface IUnitOfWork : IStateSaveChanges
    {
        Task<IDbContextTransaction> BeginTransactionAsync();

        IBaseRepository<User> Users { get; set; }

        IBaseRepository<Role> Roles { get; set; }

        IBaseRepository<UserRole> UserRoles { get; set; }
    }
}
