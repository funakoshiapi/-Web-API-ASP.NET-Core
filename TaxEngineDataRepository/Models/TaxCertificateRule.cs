using System;
using System.Collections.Generic;

#nullable disable

namespace TaxEngineAPI.Entity.Models
{
    public partial class TaxCertificateRule
    {
        public string Id { get; set; }
        public string CertificateId { get; set; }
        public string AttributeId { get; set; }
        public string ResultCertificateId { get; set; }
        public string AddCertificateAid { get; set; }
        public string AddCertificateBid { get; set; }
        public string AddCertificateCid { get; set; }
        public bool? AddCertificateTagOnly { get; set; }
        public bool Replace { get; set; }
        public bool? Final { get; set; }
        public DateTime EffectiveFrom { get; set; }
        public DateTime EffectiveTo { get; set; }
        public int Priority { get; set; }

        public virtual TaxCertificate AddCertificateA { get; set; }
        public virtual TaxCertificate AddCertificateB { get; set; }
        public virtual TaxCertificate AddCertificateC { get; set; }
        public virtual AttributeTag Attribute { get; set; }
        public virtual TaxCertificate Certificate { get; set; }
        public virtual TaxCertificate ResultCertificate { get; set; }
    }
}
