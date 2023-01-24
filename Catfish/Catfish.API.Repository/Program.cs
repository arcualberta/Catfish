using Catfish.API.Repository;
using Catfish.API.Repository.Interfaces;
using Catfish.API.Repository.Services;
using Hangfire;

var builder = WebApplication.CreateBuilder(args);


// Add services to the container.

builder.Services.AddControllers()
    .AddNewtonsoftJson();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

ConfigurationManager configuration = builder.Configuration;
builder.Services.AddDbContext<RepoDbContext>(options => options.UseSqlServer(configuration.GetConnectionString("catfish")));


// MR Jan 24 2023: Hangfire
builder.Services.AddHangfire(x => x.UseSqlServerStorage(configuration.GetConnectionString("catfish")));
builder.Services.AddHangfireServer();

//Adding general Catfish extensions
builder.AddCatfishExtensions();

//Adding services specific to this project
builder.Services.AddScoped<IEntityTemplateService, EntityTemplateService>();
builder.Services.AddScoped<IEntityService, EntityService>();

builder.Services.AddScoped<ISolrService, SolrService>();
builder.Services.AddScoped<IWorkflowService, WorkflowService>();
builder.Services.AddScoped<IBackgroundJobService, BackgroundJobService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
//if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.UseCatfishExtensions();
app.Run();
