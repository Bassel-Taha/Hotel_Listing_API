using ASP_API_Udemy_Course;
using Microsoft.EntityFrameworkCore;
using Serilog;
using ASP_API_Udemy_Course.AutoMapper;
using ASP_API_Udemy_Course.Repository;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


//using cors and allowing all processes for the user 
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll" , b => b.AllowAnyHeader()
                                         .AllowAnyOrigin()
                                         .AllowAnyMethod());
});

//add services to the container

//adding the DB context to the services to start creating the DataBase  
var connectionstring = builder.Configuration.GetConnectionString("Hotel_Listing_DB");
builder.Services.AddDbContext<Hotel_Listing_DB_Context>(options => options.UseSqlServer(connectionstring));





//adding automapper to the services
builder.Services.AddAutoMapper(typeof(AutoMapperConfig));



// inplimenting the interfaces function of the repository from the repository class  
/// implimenting country repository
builder.Services.AddScoped<IcountryRepository, CountryRepositor>();
/// implimenting hotel repository
builder.Services.AddScoped<IhotelRepository, HotelRepositor>();



/////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
//using serilog
builder.Host.UseSerilog(
                        ( ctx , lc ) => lc.WriteTo.Console()
                        .ReadFrom.Configuration(ctx.Configuration));

var app = builder.Build();

/////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(); 
}

app.UseSerilogRequestLogging();

app.UseHttpsRedirection();

app.UseCors("AllowAll");

app.UseAuthorization();

app.MapControllers();

app.Run();
