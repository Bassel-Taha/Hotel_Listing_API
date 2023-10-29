using ASP_API_Udemy_Course.Models.data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ASP_API_Udemy_Course.Models.Configration_for_the_DB_Context
{
    public class CountriesDataConfigration : IEntityTypeConfiguration<Country>
    {
        public void Configure(EntityTypeBuilder<Country> builder)
        {
            builder.HasData(
                new Country
                {
                    Id = 1,
                    Name = "egypt",
                    ShortName = "EG",
                },
                 new Country
                {
                    Id = 2,
                    Name = "Saudia arabia",
                    ShortName = "SAR"
                },
                new Country
                {
                    Id = 3,
                    Name = "United Arab Imarates",
                    ShortName = "UAE"

                });
        }
    }
}
