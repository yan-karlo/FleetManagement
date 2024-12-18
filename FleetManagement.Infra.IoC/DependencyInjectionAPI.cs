using FleetManagement.Application.Interfaces.Services;
using FleetManagement.Application.Mappings;
using FleetManagement.Application.Services;
using FleetManagement.Domain.Interfaces.Repositories;
using FleetManagement.Infra.Data.Context;
using FleetManagement.Infra.Data.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace FleetManagement.Infra.IoC
{
    public static class DependencyInjectionAPI
    {
        public static IServiceCollection AddInfrastructureAPI(this IServiceCollection services, IConfiguration configuration)
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
