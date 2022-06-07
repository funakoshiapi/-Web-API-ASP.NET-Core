using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace TaxEngineAPI.Entity.Models
{
    public class SimpleTaxCertificateRuleComparer : IComparer<TaxCertificateRule>
    {
        public int Compare(TaxCertificateRule x, TaxCertificateRule y)
        {
            var c = new CaseInsensitiveComparer();

            if (x == null && y == null)
                return 0;
            else if (x == null)
                return -1;
            else if (y == null)
                return 1;
            else
                return c.Compare(x.Id, y.Id);
        }
    }

    public class SimpleTaxCertificateRuleEqualityComparer : IEqualityComparer<TaxCertificateRule>
    {
        public bool Equals(TaxCertificateRule x, TaxCertificateRule y)
        {
            if (x == null && y == null)
                return true;
            else if (x == null || y == null)
                return false;
            else
                return x.Id.ToUpper() == y.Id.ToUpper();
        }

        public int GetHashCode(TaxCertificateRule obj)
        {
            return obj.Id.GetHashCode();
        }
    }
}
