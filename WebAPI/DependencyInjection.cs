
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Versioning;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Serialization;
using System;
using System.IO;
using System.Reflection;
using WebAPI.App.ExceptionHandlers;
using WebAPI.App.Filters;
using WebAPI.App.Models;
using WebAPI.Infrastructure;
using WebAPI.Infrastructure.Repositories;

namespace WebAPI
{
    internal static class DependencyInjection
    {
        public static IServiceCollection AddAppSettings(this IServiceCollection services, IConfiguration configuration)
        {
            var apiSettings = new ApiSettings();
            configuration.GetSection("ApiSettings").Bind(apiSettings);
            services.AddSingleton(apiSettings);

            services.AddScoped<IExceptionHandler, ValidationExceptionHandler>();
            services.AddScoped<IExceptionHandler, InvalidModelStateExceptionHandler>();
            return services;
        }

        public static IServiceCollection AddDBRepositories(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddSqlite<AppDbContext>(configuration.GetConnectionString("testDB") ?? "Data Source=testDB.db");

            services.AddScoped<ITradeRepository, TradeRepository>();
            services.AddScoped<IStockRepository, StockRepository>();
            return services;
        }
       
        public static void EnsureDatabaseSetup(this IApplicationBuilder app)
        {
            using var scope = app.ApplicationServices.CreateScope();
            var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
            db.Database.EnsureCreated();
            Infrastructure.DataSeeder.Seed(db);
        }


        public static IServiceCollection AddControllers(this IServiceCollection services)
        {
            services.AddControllers(options =>
            {
                options.Filters.Add<ApiExceptionFilterAttribute>();
            }).AddNewtonsoftJson(options =>
            {
                options.SerializerSettings.Converters.Add(new StringEnumConverter());
                options.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
            });

            return services;
        }

        public static IServiceCollection AddSwagger(this IServiceCollection services, IConfiguration configuration)
        {
            services
            .AddApiVersioning(opt =>
            {
                opt.DefaultApiVersion = new ApiVersion(1, 0);
                opt.AssumeDefaultVersionWhenUnspecified = true;
                opt.ReportApiVersions = true;
                opt.ApiVersionReader = ApiVersionReader.Combine(new UrlSegmentApiVersionReader());
            })
            .AddVersionedApiExplorer(o =>
            {
                o.GroupNameFormat = "'v'VVV";
                o.SubstituteApiVersionInUrl = true;
            })
            .AddSwaggerGen(options =>
            {
                options.EnableAnnotations();
                options.SwaggerDoc("v1", new OpenApiInfo() { Title = "API v1", Version = "v1" });
                options.UseInlineDefinitionsForEnums();
                options.DescribeAllParametersInCamelCase();

                var assembly = Assembly.GetExecutingAssembly();
                var assemblyPath = Path.GetDirectoryName(assembly.Location);
                try
                {
                    foreach (var filePath in Directory.GetFiles(assemblyPath, $"{assembly.GetName().Name}.xml"))
                        options.IncludeXmlComments(filePath, true);
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }

                var apiKeyHeaderName = configuration["ApiSettings:ApiKeyHeaderName"];

                options.AddSecurityDefinition("ApiKey", new OpenApiSecurityScheme
                {
                    Type = SecuritySchemeType.ApiKey,
                    Name = apiKeyHeaderName,
                    In = ParameterLocation.Header,
                    Description = "API Key Authentication"
                });

                options.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                            new OpenApiSecurityScheme
                            {
                                Reference = new OpenApiReference
                                {
                                    Type = ReferenceType.SecurityScheme,
                                    Id = "ApiKey"
                                }
                            },
                            Array.Empty<string>()
                    }
                });
            })
            .AddSwaggerGenNewtonsoftSupport();

            return services;
        }

        public static IServiceCollection AddFluentValidation(this IServiceCollection services)
        {
            services.AddFluentValidation(conf =>
            {
                conf.RegisterValidatorsFromAssemblyContaining<Program>();
                conf.AutomaticValidationEnabled = false;
                conf.DisableDataAnnotationsValidation = true;
            })
            .Configure<ApiBehaviorOptions>(options =>
            {
                    options.SuppressModelStateInvalidFilter = true;
            });

            return services;
        }

    }
}
