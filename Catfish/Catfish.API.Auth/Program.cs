using AutoMapper;
using Catfish.API.Auth.Interfaces;
using Catfish.API.Auth.Models;
using Catfish.API.Auth.Services;
using CatfishExtensions;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;

var builder = WebApplication.CreateBuilder(args);
ConfigurationManager configuration = builder!.Configuration;

// Entity Framework
builder.Services.AddDbContext<AuthDbContext>(options
    => options.UseSqlServer(configuration.GetConnectionString("AuthConnectionString")!));

// Identity
builder.Services.AddIdentity<IdentityUser, IdentityRole>()
    .AddEntityFrameworkStores<AuthDbContext>()
    .AddDefaultTokenProviders();

builder.AddCatfishExtensions(true, true);

builder.Services.AddControllers();

builder.Services.AddSingleton(new MapperConfiguration(mc => mc.AddProfile(new AuthMapper())).CreateMapper());
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IAccountService, AccountService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
