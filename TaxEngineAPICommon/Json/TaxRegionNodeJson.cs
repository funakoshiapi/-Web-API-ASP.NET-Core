using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace TaxEngineAPI.Json
{
    public class TaxRegionNodeJson
    {
        [JsonProperty("node_id", Required = Required.Always)]
        public string Id { get; set; }

        [JsonProperty("node_name", Required = Required.Always)]
        public string Code { get; set; }

        [JsonProperty("description", Required = Required.Always)]
        public string Description { get; set; }

        [JsonProperty ("parent_node_id", Required = Required.AllowNull)]
        public string ParentId { get; set; }

        [JsonProperty ("has_children", Required = Required.AllowNull)]
        public bool HasChildren { get; set; }
    }
}
