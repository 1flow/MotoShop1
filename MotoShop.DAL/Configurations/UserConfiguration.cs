using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MotoShop.Domain.Entity;
using System.Reflection.Emit;


namespace MotoShop.DAL.Configurations
{
    public class UserConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure (EntityTypeBuilder<User> builder)
        {
            builder.Property(x => x.Id).ValueGeneratedOnAdd();
            builder.Property(x => x.Login).HasMaxLength(100);


            builder.HasMany(x => x.Roles)
                .WithMany(x => x.Users)
                .UsingEntity<UserRole>(
                l => l.HasOne<Role>().WithMany().HasForeignKey(x => x.RoleId),   
                l => l.HasOne<User>().WithMany().HasForeignKey(x => x.UserId)   
                );

            builder.HasData(new List<User>()
            {
                new User()
                {
                    Id = 1,
                    Login = "YamahaLover",
                    Password = "Password",
                    CreatedAt = DateTime.UtcNow
                }
            });

            

           builder.HasData(new User[]
           {
                 new User()
                 {
                      Id = 321,
                      Login = "Admin",
                      Password = "123456",

                },
           });
            
        }
    }
}
