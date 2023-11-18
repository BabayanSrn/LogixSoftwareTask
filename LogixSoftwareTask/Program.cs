using LogixSoftwareTask.BusinessLogicLayer.Interfaces;
using LogixSoftwareTask.BusinessLogicLayer.Services;
using LogixSoftwareTask.DataAccessLayer.Data;
using LogixSoftwareTask.DataAccessLayer.Interfaces;
using LogixSoftwareTask.DataAccessLayer.Repositories;
using LogixSoftwareTask.Storage.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddDbContext<LogixDbContext>(o =>
{
    o.UseSqlServer(builder.Configuration.GetConnectionString("LogixSoftwareConnection"));
});

builder.Services.AddIdentity<User, IdentityRole>()
        .AddEntityFrameworkStores<LogixDbContext>()
        .AddDefaultTokenProviders();

builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IClassesRepository, ClassesRepository>();

builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IClassesService, ClassesService>();

builder.Services.AddSwaggerGen(option =>
{
    option.SwaggerDoc("v1", new OpenApiInfo { Title = "LogixSoftwareTask", Version = "v1" });
    option.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        In = ParameterLocation.Header,
        Description = "Please enter a valid token",
        Type = SecuritySchemeType.ApiKey,
        BearerFormat = "JWT",
        Scheme = "Bearer"
    });
    option.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type=ReferenceType.SecurityScheme,
                                Id="Bearer"
                            }
                        },
                        new string[]{}
                    }
                });
});

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthorization();

app.MapControllers();

app.Run();
