using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using OpportPortal.Infrastructure.Persistence;

namespace OpportPortal.Infrastructure
{
    public static class InfrastructureModule
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddData(configuration);

            return services;
        }

        private static IServiceCollection AddData(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<OpportPortalDbContext>(options => options.UseSqlServer(configuration.GetConnectionString("OpportPortalDb")));
            return services;
        }

    }
}
