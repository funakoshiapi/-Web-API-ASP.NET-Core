using System;
using System.Collections.Generic;

#nullable disable

namespace TaxEngineAPI.Entity.Models
{
    public partial class ItemTaxGroupDetail
    {
        public string Id { get; set; }
        public string ItemTaxGroupId { get; set; }
        public string TaxCodeId { get; set; }
        public DateTime EffectiveFrom { get; set; }
        public DateTime EffectiveTo { get; set; }

        public virtual ItemTaxGroup ItemTaxGroup { get; set; }
        public virtual TaxCode TaxCode { get; set; }
    }
}
