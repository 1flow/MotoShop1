using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MotoShop.Domain.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MotoShop.DAL.Configurations
{
    public class MotorcycleConfiguration : IEntityTypeConfiguration<Motorcycle>
    {
        public void Configure(EntityTypeBuilder<Motorcycle> builder)
        {
            builder.HasData(new Motorcycle
            {
                Id = 444,
                Name = "DSA",
                Description = new string('A', 50),
                CreatedAt = DateTime.UtcNow,
                Capacity = 230,
                MotorcycleModel = Domain.Enum.MotorcycleModel.Yamaha,
                MotorcycleType = Domain.Enum.MotorcycleType.Offroad

            });

            builder.HasData(new Motorcycle
            {
                Id = 6666,
                Name = "SUPERHAYABUSA",
                Description = new string('X', 12),
                CreatedAt = DateTime.UtcNow,
                Capacity = 2222,
                MotorcycleModel = Domain.Enum.MotorcycleModel.Suzuki,
                MotorcycleType = Domain.Enum.MotorcycleType.Sport

            });
        }
    }
}
