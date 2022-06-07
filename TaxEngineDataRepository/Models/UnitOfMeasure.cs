using System;
using System.Collections.Generic;

#nullable disable

namespace TaxEngineAPI.Entity.Models
{
    public partial class UnitOfMeasure
    {
        public UnitOfMeasure()
        {
            TaxCodes = new HashSet<TaxCode>();
        }

        public string UoM { get; set; }
        public string Description { get; set; }

        public virtual ICollection<TaxCode> TaxCodes { get; set; }
    }
}
