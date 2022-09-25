namespace Infrastructure.Location.Models
{
    public class CurrentWeather
    {
        public string? Time { get; set; }

        /// <summary>
        /// Temperature in <see cref="WeatherForecastOptions.Temperature_Unit"/>
        /// </summary>
        public float Temperature { get; set; }

        /// <summary>
        /// WMO Weather interpretation code.
        /// To get an actual string representation use <see cref="LocationService.WeathercodeToString(int)"/>
        /// </summary>
        public float Weathercode { get; set; }

        /// <summary>
        /// Windspeed. Unit defined in <see cref="WeatherForecastOptions.Windspeed_Unit"/>
        /// </summary>
        /// <value></value>
        public float Windspeed { get; set; }

        /// <summary>
        /// Wind direction in degrees
        /// </summary>
        public float WindDirection { get; set; }
    }
}
