using System;
using System.Collections.Generic;

#nullable disable

namespace TaxEngineAPI.Entity.Models
{
    public partial class TaxRegion
    {
        public TaxRegion()
        {
            InverseParentRegion = new HashSet<TaxRegion>();
            LogisticLocationRegions = new HashSet<LogisticLocationRegion>();
            TaxCertificates = new HashSet<TaxCertificate>();
            TaxCodeRegions = new HashSet<TaxCodeRegion>();
        }

        public string Id { get; set; }
        public string TaxRegionCode { get; set; }
        public string TaxRegionDescription { get; set; }
        public string ParentRegionId { get; set; }

        public virtual TaxRegion ParentRegion { get; set; }
        public virtual ICollection<TaxRegion> InverseParentRegion { get; set; }
        public virtual ICollection<LogisticLocationRegion> LogisticLocationRegions { get; set; }
        public virtual ICollection<TaxCertificate> TaxCertificates { get; set; }
        public virtual ICollection<TaxCodeRegion> TaxCodeRegions { get; set; }
    }
}
