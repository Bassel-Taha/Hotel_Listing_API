using ASP_API_Udemy_Course.data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ASP_API_Udemy_Course.Models.Configration_for_the_DB_Context
{
    public class HotelsDataConfigration : IEntityTypeConfiguration<Hotel>
    {
        public void Configure(EntityTypeBuilder<Hotel> builder)
        {
            builder.HasData(
                new Hotel
                {
                    Id = 1,
                    Name = "the clock tower",
                    Address = "MAKA",
                    CountryId = 2,
                    Rating = 5
                },
                new Hotel
                {
                    Id = 2,
                    Name = "Burj Khalifa",
                    Address = "Dubai",
                    CountryId = 3,
                    Rating = 5

                },
                new Hotel
                {
                    Id = 3,
                    Name = "The Four Seasoons",
                    Address = "Alexandrai",
                    CountryId = 1,
                    Rating = 4
                },
                new Hotel
                {
                    Id = 4,
                    Name = "Almadina Hotel",
                    Address = "Almadina Almonawara",
                    CountryId = 2,
                    Rating = 5

                }
                );
        }
    }
}
