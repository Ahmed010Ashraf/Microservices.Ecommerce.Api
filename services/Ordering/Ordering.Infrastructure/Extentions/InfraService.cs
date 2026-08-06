using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Ordering.Core.Repositories;
using Ordering.Infrastructure.Data;
using Ordering.Infrastructure.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ordering.Infrastructure.Extentions
{
    public static class InfraService
    {
        public static IServiceCollection AddInfraService(this IServiceCollection services , IConfiguration config) { 
        
            services.AddDbContext<OrderContext>(options =>
            {
                options.UseSqlServer(config.GetConnectionString("OrderingConnectionString"),

                sqloptions => sqloptions.EnableRetryOnFailure()
                    );
            });

            services.AddScoped(typeof(IAsyncRepository<>), typeof(RepositoryBase<>));
            services.AddScoped<IOrderRepository, OrderRepository>();
            return services;
        }
    }
}
