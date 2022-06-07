using System;
using System.Collections.Generic;

#nullable disable

namespace TaxEngineAPI.Entity.Models
{
    public partial class TaxCertificate
    {
        public TaxCertificate()
        {
            CustomerTaxCertificates = new HashSet<CustomerTaxCertificate>();
            TaxCertificateRuleAddCertificateAs = new HashSet<TaxCertificateRule>();
            TaxCertificateRuleAddCertificateBs = new HashSet<TaxCertificateRule>();
            TaxCertificateRuleAddCertificateCs = new HashSet<TaxCertificateRule>();
            TaxCertificateRuleCertificates = new HashSet<TaxCertificateRule>();
            TaxCertificateRuleResultCertificates = new HashSet<TaxCertificateRule>();
            TaxExemptionRules = new HashSet<TaxExemptionRule>();
        }

        public string Id { get; set; }
        public string CertificateId { get; set; }
        public string CertificateTypeId { get; set; }
        public string TaxRegionId { get; set; }
        public string Description { get; set; }
        public bool? Active { get; set; }
        public bool Dynamic { get; set; }
        public bool Manual { get; set; }
        public int MajorLevel { get; set; }
        public int MinorLevel { get; set; }

        public virtual TaxCertificateType CertificateType { get; set; }
        public virtual TaxRegion TaxRegion { get; set; }
        public virtual ICollection<CustomerTaxCertificate> CustomerTaxCertificates { get; set; }
        public virtual ICollection<TaxCertificateRule> TaxCertificateRuleAddCertificateAs { get; set; }
        public virtual ICollection<TaxCertificateRule> TaxCertificateRuleAddCertificateBs { get; set; }
        public virtual ICollection<TaxCertificateRule> TaxCertificateRuleAddCertificateCs { get; set; }
        public virtual ICollection<TaxCertificateRule> TaxCertificateRuleCertificates { get; set; }
        public virtual ICollection<TaxCertificateRule> TaxCertificateRuleResultCertificates { get; set; }
        public virtual ICollection<TaxExemptionRule> TaxExemptionRules { get; set; }
    }
}
