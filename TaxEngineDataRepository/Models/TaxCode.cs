using System;
using System.Collections.Generic;

#nullable disable

namespace TaxEngineAPI.Entity.Models
{
    public partial class TaxCode
    {
        public TaxCode()
        {
            ItemTaxGroupDetails = new HashSet<ItemTaxGroupDetail>();
            TaxCodeRegions = new HashSet<TaxCodeRegion>();
            TaxExemptionRuleAddTaxCodeAs = new HashSet<TaxExemptionRule>();
            TaxExemptionRuleAddTaxCodeBs = new HashSet<TaxExemptionRule>();
            TaxExemptionRuleAddTaxCodeCs = new HashSet<TaxExemptionRule>();
            TaxExemptionRuleResultTaxCodes = new HashSet<TaxExemptionRule>();
            TaxExemptionRuleTaxCodes = new HashSet<TaxExemptionRule>();
            TaxRates = new HashSet<TaxRate>();
        }

        public string Id { get; set; }
        public string Code { get; set; }
        public string Description { get; set; }
        public string UoM { get; set; }
        public int MajorLevel { get; set; }
        public int MinorLevel { get; set; }

        public virtual UnitOfMeasure UoMNavigation { get; set; }
        public virtual ICollection<ItemTaxGroupDetail> ItemTaxGroupDetails { get; set; }
        public virtual ICollection<TaxCodeRegion> TaxCodeRegions { get; set; }
        public virtual ICollection<TaxExemptionRule> TaxExemptionRuleAddTaxCodeAs { get; set; }
        public virtual ICollection<TaxExemptionRule> TaxExemptionRuleAddTaxCodeBs { get; set; }
        public virtual ICollection<TaxExemptionRule> TaxExemptionRuleAddTaxCodeCs { get; set; }
        public virtual ICollection<TaxExemptionRule> TaxExemptionRuleResultTaxCodes { get; set; }
        public virtual ICollection<TaxExemptionRule> TaxExemptionRuleTaxCodes { get; set; }
        public virtual ICollection<TaxRate> TaxRates { get; set; }
    }
}
