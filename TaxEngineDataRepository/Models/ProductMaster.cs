using System;
using System.Collections.Generic;

#nullable disable

namespace TaxEngineAPI.Entity.Models
{
    public partial class ProductMaster
    {
        public string Id { get; set; }
        public string ProductCode { get; set; }
        public string ProductDescription { get; set; }
    }
}
