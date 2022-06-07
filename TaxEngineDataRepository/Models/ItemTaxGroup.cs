using System;
using System.Collections.Generic;

#nullable disable

namespace TaxEngineAPI.Entity.Models
{
    public partial class ItemTaxGroup
    {
        public ItemTaxGroup()
        {
            ItemTaxGroupDetails = new HashSet<ItemTaxGroupDetail>();
        }

        public string Id { get; set; }
        public string ItemTaxGroupCode { get; set; }
        public string ItemTaxGroupDescription { get; set; }

        public virtual ICollection<ItemTaxGroupDetail> ItemTaxGroupDetails { get; set; }
    }
}
