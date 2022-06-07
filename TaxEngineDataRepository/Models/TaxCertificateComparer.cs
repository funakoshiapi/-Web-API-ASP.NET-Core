using System.Collections;
using System.Collections.Generic;

namespace TaxEngineAPI.Entity.Models
{
    public class SimpleTaxCertificateComparer : IComparer<TaxCertificate>
    {
        public int Compare(TaxCertificate x, TaxCertificate y)
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

    public class SimpleTaxCertificateEqualityComparer : IEqualityComparer<TaxCertificate>
    {
        public bool Equals(TaxCertificate x, TaxCertificate y)
        {
            if (x == null && y == null)
                return true;
            else if (x == null || y == null)
                return false;
            else
                return x.Id.ToUpper() == y.Id.ToUpper();
        }

        public int GetHashCode(TaxCertificate obj)
        {
            return obj.Id.GetHashCode();
        }
    }
}