using System.Text.Json.Serialization;

namespace Infrastructure.Location.Models;

/// <summary>
/// Returned by Geocoding Api.
/// </summary>
public class LocationData
{
    /// <summary>
    /// Unique identifier for this exact location
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// Location name. Localized following <see cref="GeocodingOptions.Language"/>
    /// </summary>
    public string? Name { get; set; }

    /// <summary>
    /// Geographical WGS84 coordinates of this location
    /// </summary>
    public float Latitude { get; set; }

    /// <summary>
    /// Geographical WGS84 coordinates of this location
    /// </summary>
    public float Longitude { get; set; }

    /// <summary>
    /// Elevation above sea level in meters.
    /// </summary>
    public float Elevation { get; set; }
    public string? Timezone { get; set; }
    [JsonPropertyName("feature_code")]
    public string? FeatureCode { get; set; }
    [JsonPropertyName("country_code")]
    public string? CountyCode { get; set; }
    public string? Country { get; set; }
    public int CountyId { get; set; }
    public int Population { get; set; }
    public string[]? Postcodes { get; set; }
    
}
