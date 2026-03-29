using Sienna.Application;
using Sienna.Infrastructure;
using Sienna.Infrastructure.Migrations;
using Sienna.WebApi;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddApplication();
builder.Services.AddInfrastructure(builder.Configuration);
builder.Services.AddWebServices();

var app = builder.Build();

app.UseForwardedHeaders();

app.Services.ForceMigration();

app.MapApiReferences();

app.UseHttpsRedirection();

app.UseRouting();

app.UseCors("VueApp");

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.MapGet("/", () => Results.Redirect("/scalar")).ExcludeFromDescription();

app.Run();
