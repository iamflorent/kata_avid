using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Infrastructure.Location.Models
{
    public class WeatherForecastResponse
    {
        /// <summary>
        /// WGS84 of the center of the weather grid-cell which was used to generate this forecast. 
        /// This coordinate might be up to 5 km away.
        /// </summary>
        public float Latitude { get; set; }

        /// <summary>
        /// WGS84 of the center of the weather grid-cell which was used to generate this forecast. 
        /// This coordinate might be up to 5 km away.
        /// </summary>
        public float Longitude { get; set; }

        /// <summary>
        /// The elevation in meters of the selected weather grid-cell.
        /// </summary>
        public float Elevation { get; set; }

        /// <summary>
        /// Generation time of the weather forecast in milliseconds.
        /// </summary>
        [JsonPropertyName("generationtime_ms")]
        public float GenerationTime { get; set; }

        /// <summary>
        /// Applied timezone offset from 
        /// the <see cref="WeatherForecastOptions.Timezone"/> parameter.
        /// </summary>
        /// <value></value>

        [JsonPropertyName("utc_offset_seconds")]
        public int UtcOffset { get; set; }

        /// <summary>
        /// Timezone identifier
        /// </summary>
        /// <example>Europe/Berlin</example>
        public string? Timezone { get; set; }

        /// <summary>
        /// Timezone abbreviation
        /// </summary>
        /// <example>CEST</example>

        [JsonPropertyName("timezone_abbreviation")]
        public string? TimezoneAbbreviation { get; set; }

        /// <summary>
        /// Current weather conditions
        /// </summary>

        [JsonPropertyName("current_weather")]
        public CurrentWeather? CurrentWeather { get; set; }

    }
}
