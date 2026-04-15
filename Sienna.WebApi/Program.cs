using Sienna.Application;
using Sienna.Infrastructure;
using Sienna.Infrastructure.Migrations;
using Sienna.WebApi;
using Sienna.WebApi.Endpoints;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddApplication();
builder.Services.AddInfrastructure(builder.Configuration);
builder.Services.AddWebServices();

var app = builder.Build();

//app.Services.ForceMigration();

app.UseForwardedHeaders();

app.MapApiReferences();

app.UseHttpsRedirection();

app.UseRouting();

app.UseCors("VueApp");

app.UseAuthentication();
app.UseAuthorization();

app.MapIdentityEndpoints();

app.MapGet("/", () => Results.Redirect("/scalar")).ExcludeFromDescription();

app.Run();
