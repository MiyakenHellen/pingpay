using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using PingPay.Identity.Application.Abstractions;
using PingPay.Identity.Domain.Repositories;
using PingPay.Identity.Infrastructure.Data;
using PingPay.Identity.Infrastructure.Repositories;
using PingPay.Identity.Infrastructure.Security;
using PingPay.Shared.Kernel.Abstractions;

namespace PingPay.Identity.Infrastructure.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration config)
    {
        services.AddDbContext<IdentityDbContext>(options =>
            options.UseNpgsql(config.GetConnectionString("Postgres")));

        services.AddScoped<IUnitOfWork>(sp => sp.GetRequiredService<IdentityDbContext>());

        services.AddScoped<IMerchantRepository, MerchantRepository>();
        services.AddScoped<IPasswordHasher, BCryptPasswordHasher>();
        services.AddScoped<ITokenService, JwtTokenService>();

        return services;
    }
}