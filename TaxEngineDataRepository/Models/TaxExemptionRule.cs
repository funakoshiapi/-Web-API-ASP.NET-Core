using System;
using System.Collections.Generic;

#nullable disable

namespace TaxEngineAPI.Entity.Models
{
    public partial class TaxExemptionRule
    {
        public string Id { get; set; }
        public string CertificateId { get; set; }
        public string TaxCodeId { get; set; }
        public string ResultTaxCodeId { get; set; }
        public string AddTaxCodeAid { get; set; }
        public string AddTaxCodeBid { get; set; }
        public string AddTaxCodeCid { get; set; }
        public bool? AddTaxCodeTagOnly { get; set; }
        public bool? Final { get; set; }
        public DateTime EffectiveFrom { get; set; }
        public DateTime EffectiveTo { get; set; }
        public int Priority { get; set; }

        public virtual TaxCode AddTaxCodeA { get; set; }
        public virtual TaxCode AddTaxCodeB { get; set; }
        public virtual TaxCode AddTaxCodeC { get; set; }
        public virtual TaxCertificate Certificate { get; set; }
        public virtual TaxCode ResultTaxCode { get; set; }
        public virtual TaxCode TaxCode { get; set; }
    }
}
