using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

#nullable disable

namespace TaxMappingRepository.Models
{
    public partial class MappingDataContext : DbContext
    {
        public MappingDataContext()
        {
        }

        public MappingDataContext(DbContextOptions<MappingDataContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Mapping> Mappings { get; set; }
        public virtual DbSet<MappingApproval> MappingApprovals { get; set; }
        public virtual DbSet<Source> Sources { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer("connection string");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // empty on purpose
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
