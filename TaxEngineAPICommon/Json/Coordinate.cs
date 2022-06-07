using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace TaxEngineAPI.Json
{
    public class Coordinate
    {
        [JsonProperty("lat", Required = Required.Always)]
        public string lat { get; set; }

        [JsonProperty("lng", Required = Required.Always)]
        public string lng { get; set; }
    }
}
