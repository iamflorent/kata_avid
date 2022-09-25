using Application.Contracts.Identity;
using Application.Contracts.Persistence;
using Application.Contracts.Services;
using Infrastructure.Location;
using Infrastructure.Persistence.Data;
using Infrastructure.Persistence.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace Microsoft.Extensions.DependencyInjection;

public static class ConfigureServices
{
    public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
    {        
        return AddInfrastructureServices(services, configuration.GetValue<bool>("UseInMemoryDatabase"));
    }

    public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, bool useInMemoryDatabase)
    {
        services.AddTransient<ILocationService, LocationService>();
        services.AddScoped<ITokenClaimsService, IdentityTokenClaimService>();
        
        if (useInMemoryDatabase)
        {
            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseInMemoryDatabase("AvivKataDb"));
            services.AddDbContext<AppIdentityDbContext>(options =>
                options.UseInMemoryDatabase("Identity"));
        }
        else
        {
            throw new NotImplementedException();
        }
        services.AddScoped<IApplicationDbContext>(provider => provider.GetRequiredService<ApplicationDbContext>());

        return services;
    }

    public static async Task SeedInfrastructure(this IServiceProvider services)
    {
        using var scope = services.CreateScope();
        var scopedProvider = scope.ServiceProvider;
        var userManager = scopedProvider.GetRequiredService<UserManager<ApplicationUser>>();
        var roleManager = scopedProvider.GetRequiredService<RoleManager<IdentityRole>>();

        var dbContext = scopedProvider.GetRequiredService<ApplicationDbContext>();
        await AppIdentityDbContextSeed.SeedAsync(userManager, roleManager);
        await ApplicationDbContextSeed.SeedAsync(dbContext);

    }
}