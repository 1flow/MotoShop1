﻿using MotoShop.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MotoShop.Domain.Entity
{
    public class User : IEntityId<long>, IAuditable
    {
        public string Login { get; set; }
        public string Password { get; set; }
        public List<Order> Orders { get; set; }
        public List<Role> Roles { get; set; }
        public DateTime CreatedAt { get; set; }
        public long CreatedBy { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public long? UpdatedBy { get; set; }
        public long Id { get; set; }
        public UserToken UserToken { get; set; }
    }
}
