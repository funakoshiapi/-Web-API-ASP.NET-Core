using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace MappingCommon
{
    public class ApprovalInput
    {

        [JsonProperty("icao")]
        public string Icao 
        { get; set; }

        [JsonProperty("taxRegionCode")]
        public string TaxRegionCode 
        { get; set; }

        [JsonProperty("latitude")]
        public string Latitude 
        { get; set; }

        [JsonProperty("longitude")]
        public string Longitude 
        { get; set; }

        [JsonProperty("address")]
        public string Address 
        { get; set; }
    }
}
