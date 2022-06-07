using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace TaxEngineAPI.Entity.Models
{
    public class SimpleTaxCodeComparer : IComparer<TaxCode>
    {
        public int Compare(TaxCode x, TaxCode y)
        {
            CaseInsensitiveComparer c = new CaseInsensitiveComparer();

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

    public class SimpleTaxCodeEqualityComparer : IEqualityComparer<TaxCode>
    {
        public bool Equals(TaxCode x, TaxCode y)
        {
            if (x == null && y == null)
                return true;
            else if (x == null || y == null)
                return false;
            else
                return x.Id.ToUpper() == y.Id.ToUpper();
        }

        public int GetHashCode(TaxCode obj)
        {
            return obj.Id.GetHashCode();
        }
    }
}
