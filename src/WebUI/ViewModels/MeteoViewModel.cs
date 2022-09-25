using static System.Net.WebRequestMethods;

namespace WebUI.ViewModels
{
    public class MeteoViewModel
    {
        const string url = "https://openweathermap.org/img/wn/{0}@2x.png";

        public MeteoViewModel(string location, float temperature = 0, float weatherCode = -1)
        {
            Location = location;
            Temperature = temperature;
            WeatherCode = weatherCode;
        }

        public string Location { get; }
        public float Temperature { get; }
        public float WeatherCode { get; }

        public string WeatherLabel
        {
            get
            {
                return WeathercodeToIconCode(WeatherCode).label;
            }
        }
        public string ImageUrl
        {
            get
            {
                return string.Format(url, WeathercodeToIconCode(WeatherCode).iconId);
            }
        }

       

        public (string label, string iconId) WeathercodeToIconCode(float weathercode)
        {
            
            switch (weathercode)
            {
                case 0:
                    return ("Clear sky", "01d");
                case 1:
                    return ("Mainly clear", "01d");
                case 2:
                    return ("Partly cloudy", "02d");
                case 3:
                    return ("Overcast", "04d");
                case 45:
                    return ("Fog", "50d");
                case 48:
                    return ("Depositing rime Fog", "50d");
                case 51:
                    return ("Light drizzle", "09d");
                case 53:
                    return ("Moderate drizzle", "09d");
                case 55:
                    return ("Dense drizzle", "09d");
                case 56:
                    return ("Light freezing drizzle", "09d");
                case 57:
                    return ("Dense freezing drizzle", "09d");
                case 61:
                    return ("Slight rain", "10d");
                case 63:
                    return ("Moderate rain", "10d");
                case 65:
                    return ("Heavy rain", "09d");
                case 66:
                    return ("Light freezing rain", "13d");
                case 67:
                    return ("Heavy freezing rain", "13d");
                case 71:
                    return ("Slight snow fall", "13d");
                case 73:
                    return ("Moderate snow fall", "13d");
                case 75:
                    return ("Heavy snow fall", "13d");
                case 77:
                    return ("Snow grains", "13d");
                case 80:
                    return ("Slight rain showers", "09d");
                case 81:
                    return ("Moderate rain showers", "09d");
                case 82:
                    return ("Violent rain showers", "09d");
                case 85:
                    return ("Slight snow showers", "09d");
                case 86:
                    return ("Heavy snow showers", "09d");
                case 95:
                    return ("Thunderstorm", "11d");
                case 96:
                    return ("Thunderstorm with light hail", "11d");
                case 99:
                    return ("Thunderstorm with heavy hail", "11d");
                default:
                    return ("Invalid weathercode", "50d");
            }
        }
    }
}
