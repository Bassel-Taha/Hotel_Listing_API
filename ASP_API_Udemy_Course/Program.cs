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
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
//using JWT with swagger
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new() { Title = "ASP_API_Udemy_Course", Version = "v1" });
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = @"JWT Authorization header using the Bearer scheme. 
                      Enter 'Bearer' [space] and then your token in the text input below.
                      Example: 'Bearer 12345abcdef'",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer"
    });
    c.AddSecurityRequirement(new OpenApiSecurityRequirement()
    {
      {
        new OpenApiSecurityScheme
        {
          Reference = new OpenApiReference
          {
            Type = ReferenceType.SecurityScheme,
            Id = "Bearer"
          },
          Scheme = "oauth2",
          Name = "Bearer",
          In = ParameterLocation.Header,

        },
        new List<string>()
      }
    });
});


//adding the IdentityCore to the services
builder.Services.AddIdentityCore<ApiUser>()
                .AddRoles<IdentityRole>()
                .AddTokenProvider<DataProtectorTokenProvider<ApiUser>>("HotelListing_API")
                .AddEntityFrameworkStores<Hotel_Listing_DB_Context>()
                .AddDefaultTokenProviders();


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

// adding body cashing to the services to inhance the performance of the API with the downside of using more memory
builder.Services.AddResponseCaching(options =>
{
    options.MaximumBodySize = 1024;
    options.UseCaseSensitivePaths = true;
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
//adding middleware for the response cashing
app.UseResponseCaching();
//writting the code for the middleware for the response cashing
//adding the middleware for the response cashing
app.Use(async (context , next) =>
{
  context.Response.GetTypedHeaders().CacheControl =
      new Microsoft.Net.Http.Headers.CacheControlHeaderValue()
      {
          Public = true,
          MaxAge = TimeSpan.FromSeconds(3)
      };
    //the cash may vary from one user to another
    context.Response.Headers[Microsoft.Net.Http.Headers.HeaderNames.Vary] =
        new string[] { "Accept-Encoding" }; 
    await next();
});

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
