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

                WeatherForecastResponse weatherForecast = (await JsonSerializer.DeserializeAsync<WeatherForecastResponse>(await response.Content.ReadAsStreamAsync(), new JsonSerializerOptions() { PropertyNameCaseInsensitive = true })) ?? new WeatherForecastResponse();

                CurrentWeather currentWeather = weatherForecast.CurrentWeather ?? new CurrentWeather();
                return new GetMeteoInfoDto()
                {
                    Temperature = currentWeather.Temperature,
                    Weathercode = currentWeather.Weathercode,
                    WindDirection = currentWeather.WindDirection,
                    Windspeed = currentWeather.Windspeed,
                    WeatherLabel = WeathercodeToString((int)currentWeather.Weathercode)
                };


            }
            catch (HttpRequestException e)
            {
                Console.WriteLine(e.Message);
                Console.WriteLine(e.StackTrace);
                
            }
            return null;
        }

        public string WeathercodeToString(int weathercode)
        {
            return weathercode switch
            {
                0 => "Clear sky",
                1 => "Mainly clear",
                2 => "Partly cloudy",
                3 => "Overcast",
                45 => "Fog",
                48 => "Depositing rime Fog",
                51 => "Light drizzle",
                53 => "Moderate drizzle",
                55 => "Dense drizzle",
                56 => "Light freezing drizzle",
                57 => "Dense freezing drizzle",
                61 => "Slight rain",
                63 => "Moderate rain",
                65 => "Heavy rain",
                66 => "Light freezing rain",
                67 => "Heavy freezing rain",
                71 => "Slight snow fall",
                73 => "Moderate snow fall",
                75 => "Heavy snow fall",
                77 => "Snow grains",
                80 => "Slight rain showers",
                81 => "Moderate rain showers",
                82 => "Violent rain showers",
                85 => "Slight snow showers",
                86 => "Heavy snow showers",
                95 => "Thunderstorm",
                96 => "Thunderstorm with light hail",
                99 => "Thunderstorm with heavy hail",
                _ => "Invalid weathercode",
            };
        }
    }
}
