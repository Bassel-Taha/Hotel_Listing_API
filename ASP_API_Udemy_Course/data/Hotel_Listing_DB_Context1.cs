using ASP_API_Udemy_Course.data;
using Microsoft.EntityFrameworkCore;

public class Hotel_Listing_DB_Context : DbContext
{
    public Hotel_Listing_DB_Context(DbContextOptions options) : base(options)
    {

    }

    public DbSet<Hotel> hotels { get; set; }

    public DbSet<Country> countries { get; set; }


    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.Entity<Country>().HasData(
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

            }
            );

        modelBuilder.Entity<Hotel>().HasData(
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
            ) ;
    }

    
}