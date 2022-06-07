using System;
using System.Collections.Generic;

#nullable disable

namespace TaxEngineAPI.Entity.Models
{
    public partial class LogisticLocation
    {
        public LogisticLocation()
        {
            LogisticLocationRegions = new HashSet<LogisticLocationRegion>();
        }

        public string Id { get; set; }
        public string LocationCode { get; set; }
        public string LocationDescription { get; set; }
        public decimal? Latitude { get; set; }
        public decimal? Longitude { get; set; }

        public virtual ICollection<LogisticLocationRegion> LogisticLocationRegions { get; set; }
    }
}
