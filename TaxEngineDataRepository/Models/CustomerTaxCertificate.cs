using System;
using System.Collections.Generic;

#nullable disable

namespace TaxEngineAPI.Entity.Models
{
    public partial class CustomerTaxCertificate
    {
        public string Id { get; set; }
        public string CustomerId { get; set; }
        public string TaxCertificateId { get; set; }
        public DateTime ValidFrom { get; set; }
        public DateTime ValidTo { get; set; }

        public virtual CustomerMaster Customer { get; set; }
        public virtual TaxCertificate TaxCertificate { get; set; }
    }
}
