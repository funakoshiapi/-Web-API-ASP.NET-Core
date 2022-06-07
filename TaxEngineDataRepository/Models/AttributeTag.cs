using System;
using System.Collections.Generic;

#nullable disable

namespace TaxEngineAPI.Entity.Models
{
    public partial class AttributeTag
    {
        public AttributeTag()
        {
            TaxCertificateRules = new HashSet<TaxCertificateRule>();
        }

        public string Id { get; set; }
        public string Tag { get; set; }
        public string Description { get; set; }
        public int MajorLevel { get; set; }
        public int MinorLevel { get; set; }

        public virtual ICollection<TaxCertificateRule> TaxCertificateRules { get; set; }
    }
}
