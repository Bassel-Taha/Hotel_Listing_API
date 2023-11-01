using ASP_API_Udemy_Course;
using Microsoft.EntityFrameworkCore;
using Serilog;
using ASP_API_Udemy_Course.AutoMapper;
using ASP_API_Udemy_Course.Repository;
using Microsoft.AspNetCore.Identity;
using ASP_API_Udemy_Course.Contract;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


//adding the IdentityCore to the services
builder.Services.AddIdentityCore<ApiUser>()
                .AddRoles<IdentityRole>()
                .AddEntityFrameworkStores<Hotel_Listing_DB_Context>();


//using cors and allowing all processes for the user 
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll" , b => b.AllowAnyHeader()
                                         .AllowAnyOrigin()
                                         .AllowAnyMethod());
});


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
/// iplimenting the auth manager
builder.Services.AddScoped<IAuthManager, AuthManager>();

//configuring the authentication JWTbearer
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme; //bearer token
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = builder.Configuration["JWtAuthentication:Issuer"],
        ValidAudience = builder.Configuration["JWtAuthentication:Audience"],
        ClockSkew = TimeSpan.Zero,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["JWtAuthentication:Key"]))
    };
});



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
