
using CatfishExtensions.Helpers;
using CatfishExtensions.Interfaces;
using Microsoft.AspNetCore.Http;

var builder = WebApplication.CreateBuilder(args);

builder.AddPiranha(options =>
{
    /**
     * This will enable automatic reload of .cshtml
     * without restarting the application. However since
     * this adds a slight overhead it should not be
     * enabled in production.
     */
    options.AddRazorRuntimeCompilation = true;

    options.UseCms();
    options.UseManager();

    options.UseFileStorage(naming: Piranha.Local.FileStorageNaming.UniqueFolderNames);
    options.UseImageSharp();
    options.UseTinyMCE();
    options.UseMemoryCache();

    var connectionString = builder.Configuration.GetConnectionString("catfish");
    options.UseEF<SQLServerDb>(db => db.UseSqlServer(connectionString));//.UseSqlite(connectionString));
    options.UseIdentityWithSeed<IdentitySQLServerDb>(db => db.UseSqlServer(connectionString));


 
    /**
     * Here you can configure the different permissions
     * that you want to use for securing content in the
     * application.
    options.UseSecurity(o =>
    {
        o.UsePermission("WebUser", "Web User");
    });
     */

    /**
     * Here you can specify the login url for the front end
     * application. This does not affect the login url of
     * the manager interface.
    options.LoginUrl = "login";
     */
});

builder.Services.AddCatfishWebExtensions();
builder.Services.AddAuthentication()
            .AddGoogle(options =>
            {
                IConfigurationSection googleAuthNSection =
                    builder.Configuration.GetSection("GoogleExternalLogin");

                options.ClientId = googleAuthNSection["Catfish2Oauth-ClientId"];
                options.ClientSecret = googleAuthNSection["Catfish2Oauth-ClientSecret"];

            });


//Adding general Catfish extensions
builder.AddCatfishExtensions();
builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromSeconds(10);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});


ConfigHelper.Initialize(builder.Configuration);
var app = builder.Build();

var enableRemoteErrors = builder.Configuration.GetSection("SiteConfig:RemoteErrors").Get<bool>();
if (app.Environment.IsDevelopment() | enableRemoteErrors)
{
    app.UseDeveloperExceptionPage();
}

app.UseCookiePolicy();
app.UseSession();

app.UsePiranha(options =>
{
    // Initialize Piranha
    App.Init(options.Api);


    // Build content types
    new ContentTypeBuilder(options.Api)
        .AddAssembly(typeof(Program).Assembly)
        .AddAssembly(typeof(CatfishWebsite).Assembly)
        .Build()
        .DeleteOrphans();

    // Configure Tiny MCE
    EditorConfig.FromFile("editorconfig.json");

    options.UseManager();
    options.UseTinyMCE();
    options.UseIdentity();
});

app.UseCatfishWebExtensions();

app.MapPost("/google", async ([FromBody] string jwt, IGoogleIdentity googleIdentity, IConfiguration configuration, HttpRequest request) =>
{
    try
    {
        var result = await googleIdentity.GetUserLoginResult(jwt);
        if (configuration.GetValue<bool>("Google:UseSession"))
        {
            request.HttpContext.Session.SetString("LoginResult", JsonSerializer.Serialize(result));
        }

        return result;
    }
    catch (Exception ex)
    {
        return new LoginResult();
    }
});

app.Run();