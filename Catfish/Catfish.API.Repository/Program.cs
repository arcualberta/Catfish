using AutoMapper;
using Catfish.API.Repository;
using Catfish.API.Repository.Interfaces;
using Catfish.API.Repository.Models.Entity;
using Catfish.API.Repository.Services;
using CatfishExtensions;
using Hangfire;

var builder = WebApplication.CreateBuilder(args);
ConfigurationManager configuration = builder.Configuration;

// Entity Framework
builder.Services.AddDbContext<RepoDbContext>(options
    => options.UseSqlServer(configuration.GetConnectionString("RepoConnectionString")!));

builder.AddCatfishExtensions(true, true);



builder.Services.AddControllers().AddNewtonsoftJson();

builder.Services.AddHangfire(x => x.UseSqlServerStorage(configuration.GetConnectionString("RepoConnectionString")));
builder.Services.AddHangfireServer();

var autoMappconfiguration = new MapperConfiguration(cfg => {
    cfg.CreateMap<string, int>().ConvertUsing(s => Convert.ToInt32(s));
    cfg.CreateMap<string, DateTime>().ConvertUsing(s=>System.Convert.ToDateTime(s));
   // cfg.CreateMap<string, eState>().ConvertUsing<eState>();
    cfg.AddProfile(new EntityMapper());
    cfg.AddProfile(new FormMapper());
});

builder.Services.AddSingleton(autoMappconfiguration.CreateMapper());
//builder.Services.AddSingleton(new MapperConfiguration(mc => mc.AddProfile(new EntityMapper())).CreateMapper());
//builder.Services.AddSingleton(new MapperConfiguration(mc => mc.AddProfile(new FormMapper())).CreateMapper());

//Adding services specific to this project
builder.Services.AddScoped<IEntityTemplateService, EntityTemplateService>();
builder.Services.AddScoped<IEntityService, EntityService>();
builder.Services.AddScoped<ISolrService, SolrService>();
builder.Services.AddScoped<IWorkflowService, WorkflowService>();
//builder.Services.AddScoped<IEmailService, EmailService>();
builder.Services.AddScoped<IBackgroundJobService, BackgroundJobService>();
builder.Services.AddScoped<IExcelFileProcessingService, ExcelFileProcessingService>();
builder.Services.AddScoped<IEmailService, EmailService>();

var app = builder.Build();

app.UseCatfishExtensions(true, true);

// Configure the HTTP request pipeline.
app.UseHttpsRedirection();

app.MapControllers();

app.UseHangfireDashboard("/hangfire", new DashboardOptions
{
    //Authorization = new[] { new MyAuthorizationFilter() }
});

app.Run();
