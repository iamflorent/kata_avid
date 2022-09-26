using Application.Contracts.Identity;
using Infrastructure.Persistence.Identity;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using WebApi.Tests.Fixtures;

namespace WebApi.Tests.ControllerApis
{
    public abstract class AbstractControllerTest : IClassFixture<CustomWebApplicationFactory<WebMarker>>, IAsyncLifetime
    {
        public CustomWebApplicationFactory<WebMarker> Factory { get; }

        public AbstractControllerTest(CustomWebApplicationFactory<WebMarker> factory) => Factory = factory;

        static readonly SemaphoreSlim SemaphoreSlim = new(1, 1);
        public async Task InitializeAsync()
        {
            await SemaphoreSlim.WaitAsync();// Fixtures : avoid parallel trouble
            try
            {
                using var scope = Factory.Services.CreateScope();

                var tokenClaimsService = scope.ServiceProvider.GetRequiredService<ITokenClaimsService>();
                var defaultUserToken = await tokenClaimsService.GetTokenAsync(AppIdentityDbContextSeed.DefaultUserName);
                var client = Factory.CreateClient();
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("bearer", defaultUserToken);
                DefaultUserClient = client;

                var adminToken = await tokenClaimsService.GetTokenAsync(AppIdentityDbContextSeed.AdminUserName);
                var adminClient = Factory.CreateClient();
                adminClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("bearer", adminToken);
                AdminClient = adminClient;
            }
            finally
            {

                SemaphoreSlim.Release();
            }
           
        }

        public Task DisposeAsync()
        {
            return Task.FromResult(0);
        }

        protected HttpClient DefaultUserClient { get; set; } = null!;


        protected HttpClient AdminClient { get; set; } = null!;
    }
}
