using System;
using System.Collections.Generic;

#nullable disable

namespace TaxEngineAPI.Entity.Models
{
    public partial class AirportGoogleLocation
    {
        public string Icao { get; set; }
        public decimal Latitude { get; set; }
        public decimal Longitude { get; set; }
        public string Country2 { get; set; }
        public string State { get; set; }
        public string County { get; set; }
        public string City { get; set; }
    }
}
