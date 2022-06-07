using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace TaxEngineAPI.Json.TtrShape
{
    public class Rate
    {
        [JsonProperty("rateId", Required = Required.Always) ]
        public string RateId { get; set; }
    }
}
