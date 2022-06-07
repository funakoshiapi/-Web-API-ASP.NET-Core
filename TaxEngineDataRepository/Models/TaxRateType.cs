using System;
using System.Collections.Generic;

#nullable disable

namespace TaxEngineAPI.Entity.Models
{
    public partial class TaxRateType
    {
        public TaxRateType()
        {
            TaxRates = new HashSet<TaxRate>();
        }

        public string RateType { get; set; }
        public string RateTypeDescription { get; set; }

        public virtual ICollection<TaxRate> TaxRates { get; set; }
    }
}
