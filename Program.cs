using BookStoreApp.Model.Entities;
using BookStoreApp.Model.Extensions;
using BookStoreApp.Model.Interface;
using BookStoreApp.Model.Repository;
using BookStoreApp.Model.Service;
using BookStoreApp.Model.Shared;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Any;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "My API", Version = "v1" });
    c.MapType<UserRole>(() => new OpenApiSchema
    {
        Type = "integer",
        Enum = new List<IOpenApiAny>
        {
            new OpenApiInteger((int)UserRole.Admin),
            new OpenApiInteger((int)UserRole.Customer)
        }
    });
});


builder.Services.AddControllers();
builder.Services.AddDIContainer();

builder.Services.AddAutoMapper(typeof(Program));

builder.Services.AddDbContext<AppDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("SqlServerConnection"));
}
);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
