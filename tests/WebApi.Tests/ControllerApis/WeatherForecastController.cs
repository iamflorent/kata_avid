using Application.Features.Location.Queries.GetMeteoInfo;
using FluentAssertions;
using System.Net.Http.Json;
using WebApi.Tests.Fixtures;

namespace WebApi.Tests.ControllerApis
{

    public class WeatherForecastController : AbstractControllerTest
    {
        
        private const string GET_METEO_ROUTE = "/api/WeatherForecast";
       
        public WeatherForecastController(CustomWebApplicationFactory<WebMarker> factory) : base(factory)
        {

        }

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
