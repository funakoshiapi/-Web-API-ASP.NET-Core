using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace TaxEngineAPI.Json.TtrShape
{
    public class ShapeRestricted
    {
        [JsonProperty("shapeId", Required = Required.Always)]
        public string ShapeId { get; set; }

        [JsonProperty("shapeName", Required = Required.Always)]
        public string ShapeName { get; set; }

        [JsonProperty("jurisdictionName", Required = Required.AllowNull)]
        public string JurisdictionName { get; set; }


    }
}
