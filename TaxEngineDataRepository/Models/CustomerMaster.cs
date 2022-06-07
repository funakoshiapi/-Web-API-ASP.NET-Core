using System;
using System.Collections.Generic;

#nullable disable

namespace TaxEngineAPI.Entity.Models
{
    public partial class CustomerMaster
    {
        public CustomerMaster()
        {
            CustomerTaxCertificates = new HashSet<CustomerTaxCertificate>();
        }

        public string Id { get; set; }
        public string CustomerNumber { get; set; }
        public string CustomerName { get; set; }

        public virtual ICollection<CustomerTaxCertificate> CustomerTaxCertificates { get; set; }
    }
}
