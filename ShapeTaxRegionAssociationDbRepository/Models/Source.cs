using System;
using System.Collections.Generic;

#nullable disable

namespace TaxMappingRepository.Models
{
    public partial class Source
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public DateTime? Date { get; set; }
    }
}
