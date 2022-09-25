using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Infrastructure.Location.Models;

public class GeocodingApiResponse
{
    /// <summary>
    /// Array of found locations
    /// </summary>
    [JsonPropertyName("results")]
    public LocationData[]? Locations { get; set; }

    /// <summary>
    /// Generation time of the weather forecast in milliseconds.
    /// </summary>
    public float Generationtime_ms { get; set; }
}
