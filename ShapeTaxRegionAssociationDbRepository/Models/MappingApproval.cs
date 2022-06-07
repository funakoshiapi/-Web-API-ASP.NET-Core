using System;
using System.Collections.Generic;

#nullable disable

namespace TaxMappingRepository.Models
{
    public partial class MappingApproval
    {
        public string Id { get; set; }
        public string Icao { get; set; }
        public string TaxRegion { get; set; }
        public string Latitude { get; set; }
        public string Longitude { get; set; }
        public string Address { get; set; }
        public DateTime Date { get; set; }
    }
}
