using Application.Contracts.Identity;
using Application.Features.Location.Queries.GetMeteoInfo;
using FluentAssertions;
using Infrastructure.Persistence.Identity;
using Microsoft.Extensions.DependencyInjection;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using WebApi.Tests.Fixtures;

namespace WebApi.Tests.ControllerApis
{
   
    public class WeatherForecastController : IClassFixture<CustomWebApplicationFactory<WebMarker>>, IAsyncLifetime
    {
        
        private const string GET_METEO_ROUTE = "/api/WeatherForecast";
        public CustomWebApplicationFactory<WebMarker> Factory { get; }
        public WeatherForecastController(CustomWebApplicationFactory<WebMarker> factory) => Factory = factory;

        public async Task InitializeAsync()
        {
            using var scope = Factory.Services.CreateScope();

            var tokenClaimsService = scope.ServiceProvider.GetRequiredService<ITokenClaimsService>();
            var defaultUserToken = await tokenClaimsService.GetTokenAsync(AppIdentityDbContextSeed.DefaultUserName);
            var client = Factory.CreateClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("bearer", defaultUserToken);
            DefaultUserClient = client;
        }

        public Task DisposeAsync()
        {
            return Task.FromResult(0);
        }

        HttpClient DefaultUserClient { get; set; } = null!;



        [Fact]
        public async Task Get_Unknown_Location_Should_Return_NotFound()
        {
            //prepare
            string town = "nowheretown**";

            //act  
            var actual = await DefaultUserClient.GetAsync($"{GET_METEO_ROUTE}/{town}");

            actual.StatusCode.Should().Be(System.Net.HttpStatusCode.NotFound);
        }

        [Fact]
        public async Task Get_Existing_Location_Should_Return_MeteoInfo()
        {
            //prepare
            string town = "Paris";
            //act        
            var actual = await DefaultUserClient.GetFromJsonAsync<GetMeteoInfoDto>($"{GET_METEO_ROUTE}/{town}");

            actual.Should().NotBeNull();
            actual?.Temperature.Should().BeInRange(-20, 55);


        }

        
    }
}
