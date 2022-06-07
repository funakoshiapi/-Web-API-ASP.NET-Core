using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

#nullable disable

namespace TaxEngineAPI.Entity.Models
{
    public partial class TaxEngineDbContext : DbContext
    {
        public TaxEngineDbContext()
        {
        }

        public TaxEngineDbContext(DbContextOptions<TaxEngineDbContext> options)
            : base(options)
        {
        }

        public virtual DbSet<AirportGoogleLocation> AirportGoogleLocations { get; set; }
        public virtual DbSet<AttributeTag> AttributeTags { get; set; }
        public virtual DbSet<CustomerMaster> CustomerMasters { get; set; }
        public virtual DbSet<CustomerTaxCertificate> CustomerTaxCertificates { get; set; }
        public virtual DbSet<ItemTaxGroup> ItemTaxGroups { get; set; }
        public virtual DbSet<ItemTaxGroupDetail> ItemTaxGroupDetails { get; set; }
        public virtual DbSet<LogisticLocation> LogisticLocations { get; set; }
        public virtual DbSet<LogisticLocationRegion> LogisticLocationRegions { get; set; }
        public virtual DbSet<ProductMaster> ProductMasters { get; set; }
        public virtual DbSet<TaxCertificate> TaxCertificates { get; set; }
        public virtual DbSet<TaxCertificateRule> TaxCertificateRules { get; set; }
        public virtual DbSet<TaxCertificateType> TaxCertificateTypes { get; set; }
        public virtual DbSet<TaxCode> TaxCodes { get; set; }
        public virtual DbSet<TaxCodeRegion> TaxCodeRegions { get; set; }
        public virtual DbSet<TaxExemptionRule> TaxExemptionRules { get; set; }
        public virtual DbSet<TaxRate> TaxRates { get; set; }
        public virtual DbSet<TaxRateType> TaxRateTypes { get; set; }
        public virtual DbSet<TaxRegion> TaxRegions { get; set; }
        public virtual DbSet<UnitOfMeasure> UnitOfMeasures { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {

            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        { 
            //Left empty on purpose
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
