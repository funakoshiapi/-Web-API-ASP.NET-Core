using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace TaxEngineAPI.Json
{
    public class TtrShapeRate
    {
        [JsonProperty("shapeId", Required = Required.Always)]
        public string ShapeId { get; set; }

        [JsonProperty("shapeName", Required = Required.Always)]
        public string ShapeName { get; set; }

        [JsonProperty("jurisdictionName", Required = Required.AllowNull)]
        public string JurisdictionName { get; set; }

        [JsonProperty("salesTaxValue", Required = Required.AllowNull)]
        public string SalesTaxValue { get; set; }

        [JsonProperty("useTaxValue", Required = Required.AllowNull)]
        public string UseTaxValue { get; set; }

        [JsonProperty("flateRate", Required = Required.AllowNull)]
        public string FlateRate { get; set; }
    }
}
