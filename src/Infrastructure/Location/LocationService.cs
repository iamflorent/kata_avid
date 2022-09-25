using Application.Contracts.Services;
using Application.Features.Location.Queries.GetMeteoInfo;
using Infrastructure.Location.Models;
using System.Globalization;
using System.Text.Json;

namespace Infrastructure.Location
{
    //https://github.com/AlienDwarf/open-meteo-dotnet/blob/master/OpenMeteo/OpenMeteoClient.cs
    public class LocationService : ILocationService
    {
        private readonly string _geocodeApiUrl = "https://geocoding-api.open-meteo.com/v1/search";
        private readonly string _weatherApiUrl = "https://api.open-meteo.com/v1/forecast";
        private async Task<GeocodingApiResponse?> GetGeocodingInfoAsync(string location)
        {
            try
            {
                //https://geocoding-api.open-meteo.com/v1/search?name=gareoult
                HttpResponseMessage response = await new HttpClient().GetAsync($"{_geocodeApiUrl}?name={location}");
                response.EnsureSuccessStatusCode();

                GeocodingApiResponse? geocodingData = await JsonSerializer.DeserializeAsync<GeocodingApiResponse>(await response.Content.ReadAsStreamAsync(), new JsonSerializerOptions() { PropertyNameCaseInsensitive = true });
                return geocodingData;
            }
            catch (HttpRequestException e)
            {
                Console.WriteLine("Can't find " + location + ". Please make sure that the name is valid.");
                Console.WriteLine(e.Message);
                return null;
            }

        }

        public async Task<GetMeteoInfoDto?> GetMeteoInfoAsync(string location)
        {
            var geoCodingInfo = await GetGeocodingInfoAsync(location);

            if (geoCodingInfo == null || geoCodingInfo.Locations == null)
            {
                return null;
            }

            var latitude = geoCodingInfo.Locations[0].Latitude;
            var longitude = geoCodingInfo.Locations[0].Longitude;

            try
            {
                string url = $"{_weatherApiUrl}?latitude={latitude.ToString(CultureInfo.InvariantCulture)}&longitude={longitude.ToString(CultureInfo.InvariantCulture)}&current_weather=true";
                HttpResponseMessage response = await new HttpClient().GetAsync(url);
                response.EnsureSuccessStatusCode();

                var weatherForecast = await JsonSerializer.DeserializeAsync<WeatherForecastResponse>(await response.Content.ReadAsStreamAsync(), new JsonSerializerOptions() { PropertyNameCaseInsensitive = true });
                return new GetMeteoInfoDto()
                {
                    Temperature = weatherForecast.CurrentWeather.Temperature,
                    Weathercode = weatherForecast.CurrentWeather.Weathercode,
                    WindDirection = weatherForecast.CurrentWeather.WindDirection,
                    Windspeed = weatherForecast.CurrentWeather.Windspeed,
                    WeatherLabel = WeathercodeToString((int)weatherForecast.CurrentWeather.Weathercode)
                };
            }
            catch (HttpRequestException e)
            {
                Console.WriteLine(e.Message);
                Console.WriteLine(e.StackTrace);
                return null;
            }

        }

        public string WeathercodeToString(int weathercode)
        {
            switch (weathercode)
            {
                case 0:
                    return "Clear sky";
                case 1:
                    return "Mainly clear";
                case 2:
                    return "Partly cloudy";
                case 3:
                    return "Overcast";
                case 45:
                    return "Fog";
                case 48:
                    return "Depositing rime Fog";
                case 51:
                    return "Light drizzle";
                case 53:
                    return "Moderate drizzle";
                case 55:
                    return "Dense drizzle";
                case 56:
                    return "Light freezing drizzle";
                case 57:
                    return "Dense freezing drizzle";
                case 61:
                    return "Slight rain";
                case 63:
                    return "Moderate rain";
                case 65:
                    return "Heavy rain";
                case 66:
                    return "Light freezing rain";
                case 67:
                    return "Heavy freezing rain";
                case 71:
                    return "Slight snow fall";
                case 73:
                    return "Moderate snow fall";
                case 75:
                    return "Heavy snow fall";
                case 77:
                    return "Snow grains";
                case 80:
                    return "Slight rain showers";
                case 81:
                    return "Moderate rain showers";
                case 82:
                    return "Violent rain showers";
                case 85:
                    return "Slight snow showers";
                case 86:
                    return "Heavy snow showers";
                case 95:
                    return "Thunderstorm";
                case 96:
                    return "Thunderstorm with light hail";
                case 99:
                    return "Thunderstorm with heavy hail";
                default:
                    return "Invalid weathercode";
            }
        }
    }
}
