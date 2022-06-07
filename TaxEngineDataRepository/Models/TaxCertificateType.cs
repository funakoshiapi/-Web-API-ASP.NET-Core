using System;
using System.Collections.Generic;

#nullable disable

namespace TaxEngineAPI.Entity.Models
{
    public partial class TaxCertificateType
    {
        public TaxCertificateType()
        {
            TaxCertificates = new HashSet<TaxCertificate>();
        }

        public string Id { get; set; }
        public string CertificateCode { get; set; }
        public string Description { get; set; }

        public virtual ICollection<TaxCertificate> TaxCertificates { get; set; }
    }
}
