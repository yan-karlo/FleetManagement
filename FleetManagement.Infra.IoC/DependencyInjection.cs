using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using FleetManagement.Infra.Data.Context;
using FleetManagement.Infra.Data.Repositories;
using FleetManagement.Domain.Interfaces.Repositories;
using FleetManagement.Application.Services;
using FleetManagement.Application.Interfaces.Services;
using FleetManagement.Application.Mappings;

namespace FleetManagement.Infra.IoC
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            // SQL Server
            var stringConnection = configuration.GetConnectionString("DefaultConnection");
            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(
                    stringConnection,
                    sqlServerOptionsAction: sqlOptions =>
                    {
                        sqlOptions.MigrationsAssembly(typeof(ApplicationDbContext).Assembly.FullName);
                        sqlOptions.EnableRetryOnFailure(maxRetryCount: 5, maxRetryDelay: TimeSpan.FromSeconds(30), errorNumbersToAdd: null);
                    }));

            //Repositories
            services.AddScoped<IColorRepository, ColorRepository>();
            services.AddScoped<IVehicleRepository, VehicleRepository>();
            services.AddScoped<IVehicleTypeRepository, VehicleTypeRepository>();

            //Services
            services.AddScoped<IColorService, ColorService>();
            services.AddScoped<IVehicleService, VehicleService>();
            services.AddScoped<IVehicleTypeService, VehicleTypeService>();

            //AutoMapper
            services.AddAutoMapper(typeof(DomainToDTOMappingProfile));


            return services;
        }
    }
}
