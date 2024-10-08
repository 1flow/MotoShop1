﻿using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MotoShop.Domain.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MotoShop.DAL.Configurations
{
    public class UserTokenConfiguration : IEntityTypeConfiguration<UserToken>
    {
        public void Configure(EntityTypeBuilder<UserToken> builder)
        {
            builder.Property(x => x.Id).ValueGeneratedOnAdd();
            builder.Property(x => x.RefreshToken).IsRequired();
            builder.Property(x => x.RefreshTokenExpireTime).IsRequired();

            builder.HasData(new List<UserToken>
            {
                new UserToken 
                { 
                    Id = 1,
                    RefreshToken = "Kjskdaskdkas",
                    RefreshTokenExpireTime = DateTime.UtcNow.AddDays(7),
                    UserId = 1
                }
            });
        }
    }
}
