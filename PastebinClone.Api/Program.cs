using Microsoft.EntityFrameworkCore;
using PastebinClone.Api;
using PastebinClone.Api.Application;
using PastebinClone.Api.Controllers.Examples;
using PastebinClone.Api.Infrastructure.Persistence;
using Swashbuckle.AspNetCore.Filters;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers(options =>
{
    options.InputFormatters.Insert(0, JsonPatchInputFormatter.GetJsonPatchInputFormatter());
});

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options => options.ExampleFilters());
builder.Services.AddSwaggerExamplesFromAssemblyOf<PatchAliasRequestExampleProvider>();

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("Sql")));

builder.Services.AddScoped<IAliasRepository, AliasRepository>();
builder.Services.AddScoped<IPasteRepository, PasteRepository>();

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

await MigrateDatabaseAsync(app);

await app.RunAsync();

static async Task MigrateDatabaseAsync(WebApplication app)
{
    await using var scope = app.Services.CreateAsyncScope();

    var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
    await dbContext.Database.MigrateAsync();
}