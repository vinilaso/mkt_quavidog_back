using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Cors.Infrastructure;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.IdentityModel.Tokens;
using Scalar.AspNetCore;
using Sienna.Application.Mappings.Identity;
using Sienna.WebApi.HostedServices.Email;
using Sienna.WebApi.OpenApi;

namespace Sienna.WebApi
{
    internal static class DependencyInjection
    {
        internal static IServiceCollection AddWebServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddControllers();
            services.AddCors(AddVueAppPolicy);
            AddEndpointsExplorer(services);
            AddForwardedHeaders(services);
            AddHostedServices(services);
            AddApiAuthentication(services, configuration);

            return services;
        }

        private static void AddApiAuthentication(IServiceCollection services, IConfiguration configuration)
        {
            var jwtSection = configuration.GetJWTSection();

            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                options.RequireHttpsMetadata = true;
                options.SaveToken = true;
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(jwtSection.Key),
                    
                    ValidateIssuer = true,
                    ValidIssuer = jwtSection.Issuer,

                    ValidateAudience = true,
                    ValidAudience = jwtSection.Audience,

                    ValidateLifetime = true,
                    ClockSkew = TimeSpan.Zero
                };
            });
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

        private static void AddForwardedHeaders(IServiceCollection services)
        {
            services.Configure<ForwardedHeadersOptions>(options =>
            {
                options.ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto;
                options.KnownIPNetworks.Clear();
                options.KnownProxies.Clear();
            });
        }

        private static void AddHostedServices(IServiceCollection services)
        {
            services.AddHostedService<EmailBackgroundWorker>();
        }
    }
}
