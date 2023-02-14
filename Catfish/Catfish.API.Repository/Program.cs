using Catfish.API.Repository;
using Catfish.API.Repository.Interfaces;
using Catfish.API.Repository.Services;
using CatfishExtensions;
using Hangfire;

var builder = WebApplication.CreateBuilder(args);


// Add services to the container.

builder.Services.AddControllers()
    .AddNewtonsoftJson();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
//MR Jan 26 2023 -- commented out calling swagger 
//we will try to call catfish.Extension builder.AddCatfishJwtAuthprization()
//builder.Services.AddSwaggerGen();

ConfigurationManager configuration = builder.Configuration;
builder.Services.AddDbContext<RepoDbContext>(options => options.UseSqlServer(configuration.GetConnectionString("catfish")));


// MR Jan 24 2023: Hangfire
builder.Services.AddHangfire(x => x.UseSqlServerStorage(configuration.GetConnectionString("catfish")));
builder.Services.AddHangfireServer();

//Adding Catfish extensions
builder.AddCatfishExtensions(true, true);

//Adding services specific to this project
builder.Services.AddScoped<IEntityTemplateService, EntityTemplateService>();
builder.Services.AddScoped<IEntityService, EntityService>();

builder.Services.AddScoped<ISolrService, SolrService>();
builder.Services.AddScoped<IWorkflowService, WorkflowService>();
builder.Services.AddScoped<IBackgroundJobService, BackgroundJobService>();
builder.Services.AddScoped<IExcelFileProcessingService, ExcelFileProcessingService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
//if (app.Environment.IsDevelopment())
//{
//    app.UseSwagger();
//    app.UseSwaggerUI();
//}

app.UseHttpsRedirection();

//MR Jan 26 2023 -- commented out UseAuthorization() 
//we will call UseJwtAuthorization from CatfishExtension
//app.UseAuthorization();
app.UseCatfishExtensions(true, true);

app.MapControllers();

app.UseHangfireDashboard("/hangfire", new DashboardOptions
{
    //Authorization = new[] { new MyAuthorizationFilter() }
});

app.Run();
