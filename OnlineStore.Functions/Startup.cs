using System;
using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using OnlineStore.DAL;
using OnlineStore.Services.Implementations;
using OnlineStore.Services.Interfaces;

[assembly: FunctionsStartup(typeof(OnlineStore.Functions.Startup))]

namespace OnlineStore.Functions
{
    public class Startup : FunctionsStartup
    {
        public override void Configure(IFunctionsHostBuilder builder)
        {
            // Access the configuration, specifically for local development
            var configuration = builder.GetContext().Configuration;
            var connectionString =
                configuration.GetConnectionString("SqlConnectionString")
                ?? Environment.GetEnvironmentVariable("SqlConnectionString");

            // Register services here
            builder.Services.AddDbContext<OnlineStoreContext>(options =>
                options.UseSqlServer(connectionString)
            );

            builder.Services.AddScoped<IOrderQueryService, OrderQueryService>();
            builder.Services.AddScoped<IOrderCommandService, OrderCommandService>();
            builder.Services.AddScoped<IProductService, ProductService>();
        }
    }
}
