using Hangfire;
using Showtimes.API.Interfaces;
using Showtimes.API.Services;

var builder = WebApplication.CreateBuilder(args);
ConfigurationManager configuration = builder.Configuration;

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddHangfire(x => x.UseSqlServerStorage(configuration.GetConnectionString("ShowtimeConnectionString")));
builder.Services.AddHangfireServer();

//Adding services specific to this project
builder.Services.AddScoped<IShowtimeQueryService, ShowtimeQueryService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
//if (app.Environment.IsDevelopment())
//{
    app.UseSwagger();
    app.UseSwaggerUI();
//}
app.UseHangfireDashboard("/hangfire", new DashboardOptions
{

    //here you could provide more security logic to limit access to certain individual
});

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
