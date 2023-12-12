using ASP_API_Udemy_Course.data;
using ASP_API_Udemy_Course.Models.Configration_for_the_DB_Context;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

public class Hotel_Listing_DB_Context : IdentityDbContext<ApiUser>
{
    public Hotel_Listing_DB_Context(DbContextOptions options) : base(options)
    {

    }

    public DbSet<Hotel> hotels { get; set; }

    public DbSet<Country> countries { get; set; }

    #region butting data in the DB as default data 
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.ApplyConfiguration(new IdentityRoleDataConfigration());
        modelBuilder.ApplyConfiguration(new CountriesDataConfigration());
        modelBuilder.ApplyConfiguration(new HotelsDataConfigration());
    }
    #endregion

}