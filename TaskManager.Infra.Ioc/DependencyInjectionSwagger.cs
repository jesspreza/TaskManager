using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;

namespace TaskManager.Infra.Ioc
{
    public static class DependencyInjectionSwagger
    {
        public static IServiceCollection AddInfrastructureSwagger(this IServiceCollection services)
        {
            services.AddSwaggerGen(c =>
            {
                c.AddSecurityDefinition("Bearer", new Microsoft.OpenApi.Models.OpenApiSecurityScheme()
                {
                    Name = "Authorization",
                    Type = Microsoft.OpenApi.Models.SecuritySchemeType.ApiKey,
                    BearerFormat = "JWT",
                    In = Microsoft.OpenApi.Models.ParameterLocation.Header,
                    Description = "JSON Web Token (JWT)",
                });

                c.AddSecurityRequirement(new Microsoft.OpenApi.Models.OpenApiSecurityRequirement()
                {
                    {
                        new Microsoft.OpenApi.Models.OpenApiSecurityScheme()
                        {
                            Reference = new Microsoft.OpenApi.Models.OpenApiReference()
                            {
                                Type = Microsoft.OpenApi.Models.ReferenceType.SecurityScheme,
                                Id = "Bearer"
                            }
                        },
                        new string[] {}
                    }
                });
                c.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = "Task Manager API",
                    Version = "v1",
                    Description = "API for managing tasks and time tracking",
                    Contact = new OpenApiContact
                    {
                        Name = "Jessica Preza",
                        Email = "jepfontes@gmail.com",
                        Url = new Uri("https://www.linkedin.com/in/jessica-preza-45b721130/")
                    }
                });

                var xmlFile = "TaskManager.Api.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                c.IncludeXmlComments(xmlPath);
            });

            return services;
        }
    }
}
