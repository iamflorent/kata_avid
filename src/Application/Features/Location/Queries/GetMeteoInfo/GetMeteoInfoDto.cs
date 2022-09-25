using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.Location.Queries.GetMeteoInfo
{
    public class GetMeteoInfoDto
    {
        public GetMeteoInfoDto()
        {
            WeatherLabel = String.Empty;
        }        
        /// <summary>
        /// Temperature in celsius
        /// </summary>
        public float Temperature { get; set; }

        /// <summary>
        /// WMO Weather interpretation code        
        /// </summary>
        public float Weathercode { get; set; }

        /// <summary>
        /// actual string representation of Weathercode
        /// </summary>
        public string WeatherLabel { get; set; }

        /// <summary>
        /// Windspeed in kmh
        /// </summary>
        public float Windspeed { get; set; }

        /// <summary>
        /// Wind direction in degrees
        /// </summary>
        public float WindDirection { get; set; }
    }
}
