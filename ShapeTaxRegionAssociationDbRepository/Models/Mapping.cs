using System;
using System.Collections.Generic;

#nullable disable

namespace TaxMappingRepository.Models
{
    public partial class Mapping
    {
        public string Id { get; set; }
        public string ShapeId { get; set; }
        public string TaxRegionId { get; set; }
        public string Status { get; set; }
        public string Source { get; set; }
        public DateTime Date { get; set; }
    }
}
