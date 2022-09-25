using Application.Contracts.Identity;
using Application.Tests.Mocks;
using Infrastructure.Persistence.Data;
using Microsoft.Extensions.DependencyInjection;

namespace Application.Tests.Fixtures
{
    public class ApplicationFixture
    {
        public ServiceProvider ServiceProvider { get; }
        public readonly object _locker = new object();
        public ApplicationFixture()
        {
            lock(_locker)
            {
                ServiceProvider = ConfigureServices().BuildServiceProvider();
                var db = ServiceProvider.GetRequiredService<ApplicationDbContext>();

                if (db.Database.EnsureCreated())
                {
                    ApplicationDbContextSeed.Seed(db);
                }
            }
            
            
        }

        public static ServiceCollection ConfigureServices()
        {
            var services = new ServiceCollection();
            services.AddLogging();
            services.AddInfrastructureServices(useInMemoryDatabase: true);
            services.AddApplicationServices();
            services.AddTransient<ICurrentUserService, CurrentUserServiceMock>();

            return services;
        }
    }
}
