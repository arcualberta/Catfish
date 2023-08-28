using Catfish.API.Repository;
using Catfish.API.Repository.Interfaces;
using Catfish.API.Repository.Services;
using CatfishExtensions.Services.Auth.Requirements;
using Hangfire;
using Microsoft.Extensions.DependencyInjection.Extensions;

var builder = WebApplication.CreateBuilder(args);
ConfigurationManager configuration = builder.Configuration;

// Entity Framework
builder.Services.AddDbContext<RepoDbContext>(options
    => options.UseSqlServer(configuration.GetConnectionString("RepoConnectionString")!));

builder.AddCatfishExtensions(true, true);

builder.Services.AddControllers().AddNewtonsoftJson();

builder.Services.AddHangfire(x => x.UseSqlServerStorage(configuration.GetConnectionString("RepoConnectionString")));
builder.Services.AddHangfireServer();

//Adding services specific to this project
builder.Services.AddScoped<IEntityTemplateService, EntityTemplateService>();
builder.Services.AddScoped<IEntityService, EntityService>();
builder.Services.AddScoped<ISolrService, SolrService>();
builder.Services.AddScoped<IWorkflowService, WorkflowService>();
builder.Services.AddScoped<IBackgroundJobService, BackgroundJobService>();
builder.Services.AddScoped<IExcelFileProcessingService, ExcelFileProcessingService>();
builder.Services.AddScoped<IEmailService, EmailService>();

//////Retrieving tenant info from the Auth API and adding access policies for each role in each tenant.
//////NOTE: This will require the Auth API running before starting the repository service
////var webClient = new CatfishExtensions.Services.CatfishWebClient();
////var tenantApiProxy = new CatfishExtensions.Services.Auth.TenantApiProxy(webClient, configuration);
////var tenants = await tenantApiProxy.GetTenants(0, int.MaxValue, true);

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("BelongsToTenant", policy =>
        policy.Requirements.Add(new BelongsToTenantRequirement()));
});

builder.Services.TryAddSingleton<IHttpContextAccessor, HttpContextAccessor>();
builder.Services.AddScoped<IAuthorizationHandler, MembershipHandler>();

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
