using System;
using System.Collections.Generic;

#nullable disable

namespace TaxEngineAPI.Entity.Models
{
    public partial class TaxCodeRegion
    {
        public int Id { get; set; }
        public string TaxCodeId { get; set; }
        public string TaxRegionId { get; set; }
        public DateTime EffectiveFrom { get; set; }
        public DateTime EffectiveTo { get; set; }

        public virtual TaxCode TaxCode { get; set; }
        public virtual TaxRegion TaxRegion { get; set; }
    }
}
