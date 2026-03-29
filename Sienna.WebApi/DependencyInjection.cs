using Microsoft.AspNetCore.Cors.Infrastructure;
using Scalar.AspNetCore;
using Sienna.WebApi.OpenApi;

namespace Sienna.WebApi
{
    internal static class DependencyInjection
    {
        internal static IServiceCollection AddWebServices(this IServiceCollection services)
        {
            services.AddControllers();
            services.AddCors(AddVueAppPolicy);
            AddEndpointsExplorer(services);

            return services;
        }

        internal static WebApplication MapApiReferences(this WebApplication app)
        {
            app.MapOpenApi();
            app.MapScalarApiReference(options =>
            {
                options.Authentication = new ScalarAuthenticationOptions
                {
                    PreferredSecuritySchemes = ["Bearer"]
                };
            });

            return app;
        }

        private static void AddEndpointsExplorer(IServiceCollection services)
        {
            services.AddOpenApi(options =>
            {
                options.AddDocumentTransformer<BearerSecuritySchemeDocumentTransformer>();
                options.AddOperationTransformer<BearerSecurityRequirementOperationTransformer>();
            });
        }

        private static void AddVueAppPolicy(CorsOptions options)
        {
            options.AddPolicy("VueApp", policy =>
            {
                policy.WithOrigins("https://dashboard-mkt.onrender.com", "http://localhost:5173");
                policy.AllowAnyHeader();
                policy.AllowAnyMethod();
            });
        }
    }
}
