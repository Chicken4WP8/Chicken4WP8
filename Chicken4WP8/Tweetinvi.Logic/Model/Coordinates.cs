using System.Collections.Generic;
using Newtonsoft.Json;
using Tweetinvi.Core.Interfaces.Models;

namespace Tweetinvi.Logic.Model
{
    /// <summary>
    /// Coordinates of a geographical location
    /// </summary>
    public class Coordinates : ICoordinates
    {
        public double Latitude { get; set; }
        public double Longitude { get; set; }

        public Coordinates() { }

        /// <summary>
        /// Create coordinates with its longitude and latitude
        /// </summary>
        public Coordinates(double longitude, double latitude)
        {
            Longitude = longitude; 
            Latitude = latitude;
        }

        [JsonProperty("coordinates")]
        private List<double> _coordinatesSetter
        {
            set
            {
                if (value != null)
                {
                    Longitude = value[0];
                    Latitude = value[1];
                }
            }
        }
    }
}