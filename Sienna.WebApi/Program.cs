using Sienna.Application;
using Sienna.Infrastructure;
using Sienna.Infrastructure.Migrations;
using Sienna.WebApi;
using Sienna.WebApi.OpenApi;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddApplication();
builder.Services.AddInfrastructure(builder.Configuration);
builder.Services.AddWebServices();

builder.Services.AddOpenApi(options =>
{
    options.AddDocumentTransformer<BearerSecuritySchemeDocumentTransformer>();
    options.AddOperationTransformer<BearerSecurityRequirementOperationTransformer>();
});

var app = builder.Build();

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
