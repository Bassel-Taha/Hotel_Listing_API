using ASP_API_Udemy_Course;
using Microsoft.EntityFrameworkCore;
using Serilog;

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
var connectionstring = builder.Configuration.GetConnectionString("learning_ASP_API");
builder.Services.AddDbContext<Learning_ASP_APIContext>(options => options.UseSqlServer(connectionstring));

//using serilog
builder.Host.UseSerilog(
                        ( ctx , lc ) => lc.WriteTo.Console()
                        .ReadFrom.Configuration(ctx.Configuration));

var app = builder.Build();

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