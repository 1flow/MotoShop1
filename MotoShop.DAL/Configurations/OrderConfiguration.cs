using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MotoShop.Domain.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;

namespace MotoShop.DAL.Configurations
{
    public class OrderConfiguration : IEntityTypeConfiguration<Order>
    {
        public void Configure(EntityTypeBuilder<Order> builder)
        {
            builder.HasOne(o => o.User)           
                .WithMany(u => u.Orders)       
                .HasForeignKey(o => o.UserId) 
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasMany(o => o.motorcycleList)
                .WithMany(m => m.Orders)
                .UsingEntity(j => j.ToTable("OrderMotorcycle"));
        }
    }
}
