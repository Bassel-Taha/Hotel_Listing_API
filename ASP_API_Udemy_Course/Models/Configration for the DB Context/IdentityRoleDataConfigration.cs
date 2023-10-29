using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ASP_API_Udemy_Course.Models.Configration_for_the_DB_Context
{
    public class IdentityRoleDataConfigration : IEntityTypeConfiguration<IdentityRole>
    {
        

        public void Configure(EntityTypeBuilder<IdentityRole> builder)
        {
            builder.HasData(
               new IdentityRole
               {
                   Name = "ADMIN",
                   NormalizedName = "Admin"
                
                   
               },
              new IdentityRole
              {
                  Name = "USER1",
                  NormalizedName = "User1"
                  
              });
        }
    }
}
