using Application.Contracts.Identity;
using Application.Tests.Mocks;
using Infrastructure.Persistence.Data;
using Microsoft.Extensions.DependencyInjection;

namespace Application.Tests.Fixtures
{
    public class ApplicationFixture
    {
        public ServiceProvider ServiceProvider { get; }


        static readonly object _locker = new object();
        static bool _seedFlag = false;
        public ApplicationFixture()
        {
            ServiceProvider = ConfigureServices().BuildServiceProvider();
            var db = ServiceProvider.GetRequiredService<ApplicationDbContext>();
            lock (_locker)
            {
                if (db.Database.EnsureCreated())
                {
                    if (!_seedFlag)
                    {
                        ApplicationDbContextSeed.Seed(db);
                        _seedFlag = true;
                    }

                }
            }
        }

        public static ServiceCollection ConfigureServices()
        {
            var services = new ServiceCollection();
            services.AddLogging();
            services.AddInfrastructureServices(useInMemoryDatabase: true);
            services.AddApplicationServices();
            var mock = new CurrentUserServiceMock();
            services.AddSingleton<ICurrentUserService>(x=> mock);
            services.AddSingleton(x=> mock);

            return services;
        }
    }
}
