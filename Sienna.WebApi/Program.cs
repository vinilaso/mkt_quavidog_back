using Sienna.Application;
using Sienna.Infrastructure;
using Sienna.Infrastructure.Migrations;
using Sienna.WebApi;
using Sienna.WebApi.Endpoints;
using Sienna.WebApi.Endpoints.Extensions;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddApplication();
builder.Services.AddInfrastructure(builder.Configuration);
builder.Services.AddWebServices(builder.Configuration);

var app = builder.Build();

#if DEBUG
app.Services.ForceMigration();
#endif

app.UseForwardedHeaders();

app.MapApiReferences();

app.UseHttpsRedirection();

app.UseRouting();

app.UseCors("VueApp");

app.UseAuthentication();
app.UseAuthorization();

app.MapEndpointGroups();

app.MapGet("/", () => Results.Redirect("/scalar")).ExcludeFromDescription();

app.Run();
