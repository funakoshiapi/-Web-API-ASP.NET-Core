using System;
using System.Collections.Generic;

#nullable disable

namespace TaxEngineAPI.Entity.Models
{
    public partial class TaxRate
    {
        public int Id { get; set; }
        public string TaxCodeId { get; set; }
        public string TaxRateType { get; set; }
        public string GrossNet { get; set; }
        public decimal TaxRate1 { get; set; }
        public DateTime EffectiveFrom { get; set; }
        public DateTime EffectiveTo { get; set; }

        public virtual TaxCode TaxCode { get; set; }
        public virtual TaxRateType TaxRateTypeNavigation { get; set; }
    }
}
