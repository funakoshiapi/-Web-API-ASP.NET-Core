using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace TaxEngineAPI.Json.TtrShape
{
    public class TtrShape
    {
        [JsonProperty("rate", Required = Required.Always)]
        public Rate Rate { get; set; }

        [JsonProperty("shapes", Required = Required.Always)]
        public IList<ShapeRestricted> Shapes { get; set; }
    }
}
