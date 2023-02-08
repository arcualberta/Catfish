
var builder = WebApplication.CreateBuilder(args);


builder.AddPiranha(options =>
{
    var connectionString = builder.Configuration.GetConnectionString("catfish");
    options.UseEF<SQLServerDb>(db => db.UseSqlServer(connectionString));
    options.UseIdentityWithSeed<IdentitySQLServerDb>(db => db.UseSqlServer(connectionString));
}); 



builder.Services.AddControllers();

//Adding general Catfish extensions
builder.AddCatfishExtensions(true, false);

//Adding project-specific services
builder.Services.AddScoped<ICatfishUserManager, CatfishUserManager>();

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

app.UseCatfishExtensions(true, false);
app.Run();
