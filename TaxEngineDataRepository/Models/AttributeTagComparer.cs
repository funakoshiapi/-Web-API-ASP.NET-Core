using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace TaxEngineAPI.Entity.Models
{
    public class SimpleAttributeTagComparer : IComparer<AttributeTag>
    {
        public int Compare(AttributeTag x, AttributeTag y)
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

    public class SimpleAttributeTagEqualityComparer : IEqualityComparer<AttributeTag>
    {
        public bool Equals(AttributeTag x, AttributeTag y)
        {
            if (x == null && y == null)
                return true;
            else if (x == null || y == null)
                return false;
            else
                return x.Id.ToUpper() == y.Id.ToUpper();
        }

        public int GetHashCode(AttributeTag obj)
        {
            return obj.Id.GetHashCode();
        }
    }
}
