using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using TaskManager.Infra.Data.Context;
using TaskManager.Application.Mappings;
using TaskManager.Domain.Interfaces;
using TaskManager.Infra.Data.Repositories;
using TaskManager.Application.Interfaces;
using TaskManager.Application.Services;
using TaskManager.Domain.Account;
using TaskManager.Infra.Data.Security;
using Microsoft.Extensions.Logging;

namespace TaskManager.Infra.Ioc
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<ApplicationDbContext>(options =>
            {
                options.UseSqlServer(configuration.GetConnectionString("DefaultConnection"),
                    b => b.MigrationsAssembly(typeof(ApplicationDbContext).Assembly.FullName));
            });

            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }
            ).AddJwtBearer(options =>
            {
                options.RequireHttpsMetadata = false;
                options.SaveToken = true;
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,

                    ValidIssuer = configuration["TokenConfigurations:Issuer"],
                    ValidAudience = configuration["TokenConfigurations:Audience"],
                    IssuerSigningKey = new SymmetricSecurityKey(
                        Encoding.UTF8.GetBytes(configuration["TokenConfigurations:Secret"])),
                    ClockSkew = TimeSpan.Zero
                };
            });

            services.AddAuthorization();

            services.AddScoped<IAuthenticate, AuthenticateService>();
            services.AddScoped<IPasswordHasher, PasswordHasher>();

            services.AddScoped<ICollaboratorRepository, CollaboratorRepository>();
            services.AddScoped<IProjectRepository, ProjectRepository>();
            services.AddScoped<ITaskRepository, TaskRepository>();
            services.AddScoped<ITimeTrackerRepository, TimeTrackerRepository>();
            services.AddScoped<IUserRepository, UserRepository>();

            services.AddScoped<ICollaboratorService, CollaboratorService>();
            services.AddScoped<IProjectService, ProjectService>();
            services.AddScoped<ITaskService, TaskService>();
            services.AddScoped<ITimeTrackerService, TimeTrackerService>();
            services.AddScoped<IUserService, UserService>();

            services.AddAutoMapper(typeof(EntityMappingProfile));


            return services;

        }
    }
}
