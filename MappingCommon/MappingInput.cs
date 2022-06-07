using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;


namespace MappingCommon
{
    public class MappingInput
    {
        [JsonProperty("shapeId")]
        public string ShapeId { get; set; }

        [JsonProperty("taxRegionId")]
        public string TaxRegionId { get; set; }
    }
}