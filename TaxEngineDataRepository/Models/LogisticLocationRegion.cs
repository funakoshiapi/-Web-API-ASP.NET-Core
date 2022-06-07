using System;
using System.Collections.Generic;

#nullable disable

namespace TaxEngineAPI.Entity.Models
{
    public partial class LogisticLocationRegion
    {
        public int Id { get; set; }
        public string LocationId { get; set; }
        public string TaxRegionId { get; set; }
        public DateTime EffectiveFrom { get; set; }
        public DateTime EffectiveTo { get; set; }

        public virtual LogisticLocation Location { get; set; }
        public virtual TaxRegion TaxRegion { get; set; }
    }
}
