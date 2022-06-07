using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace TaxEngineAPI.Entity.Models
{
    public partial class TaxRegion
    {
        [NotMapped]
        public ISet<TaxCode> TaxCodes
        {
            get
            {
                SortedSet<TaxCode> taxCodeSet = new SortedSet<TaxCode>(new SimpleTaxCodeComparer());

                if (TaxCodeRegions != null)
                {
                    foreach (var taxCode in TaxCodeRegions.OrderBy(x=>x.TaxCode.Code).ThenByDescending(x=>x.EffectiveTo).Select(x=>x.TaxCode))
                    {
                        if (taxCodeSet.All(x => x.Code != taxCode.Code))
                            taxCodeSet.Add(taxCode);
                    }
                }

                return taxCodeSet;
            }
        }

        [NotMapped]
        public ISet<TaxCode> ApplicableTaxCodes { get; set; } = new SortedSet<TaxCode>(new SimpleTaxCodeComparer());

        [NotMapped]
        public ISet<TaxCertificate> ApplicableCertificates { get; set; } = new SortedSet<TaxCertificate>(new SimpleTaxCertificateComparer());

        [NotMapped]
        public ISet<TaxCertificate> AvailableDynamicCertificates { get; set; } = new SortedSet<TaxCertificate>(new SimpleTaxCertificateComparer());

        [NotMapped]
        public ISet<TaxCertificate> EffectiveCertificates { get; set; } = new SortedSet<TaxCertificate>(new SimpleTaxCertificateComparer());

        [NotMapped]
        public int LevelIndex { get; set; }

        [NotMapped]
        public bool HasChildren
        {
            get {
                if (Children != null && Children.Any())
                    return true;
                else
                    return false;
            }
        }

        [NotMapped]
        public IList<TaxRegion> Children { get; set; }
        
    }

    public static class TaxRegionExtensions
    {
        public static ISet<TaxCode> TaxCodes(this IList<TaxRegion> taxRegions)
        {
            SortedSet<TaxCode> taxCodeSet = new SortedSet<TaxCode>(new SimpleTaxCodeComparer());

            foreach (var taxRegion in taxRegions)
            {
                if (taxRegion.TaxCodes != null)
                {
                    taxCodeSet.UnionWith(taxRegion.TaxCodes);
                }
            }
            return taxCodeSet;
        }

        public static IList<TaxRegion> AllocateEffectiveCertificates(this IList<TaxRegion> taxRegions,
            IList<TaxCertificate> effectiveCertificates)
        {
            if (taxRegions != null && effectiveCertificates != null)
            {
                foreach (var cert in effectiveCertificates)
                {
                    var region = taxRegions.FirstOrDefault(x => x.Id == cert.TaxRegionId);
                    if (region!=null)
                        region.EffectiveCertificates.Add(cert);
                    else 
                        throw new EffectiveCertificateUnallocatedRegionException();
                }
            }

            return taxRegions;
        }
        public static IList<string> AuditLog(this IList<TaxRegion> taxRegions)
        {
            List<string> log = new List<string>();

            if (taxRegions != null)
            {
                foreach (var taxRegion in taxRegions)
                {
                    log.AddRange(taxRegion.AuditLog());
                    log.Add("");
                }
            }
            return log;
        }

        public static IList<string> AuditLog(this TaxRegion taxRegion)
        {
            List<string> log = new List<string>();

            if (taxRegion != null)
            {
                log.Add($"Tax region {taxRegion.TaxRegionCode} [{taxRegion.Id}], level {taxRegion.LevelIndex}");
                log.Add($"Applicable certificates are:");
                if (taxRegion.ApplicableCertificates != null)
                {
                    foreach (var cert in taxRegion.ApplicableCertificates)
                    {
                        log.AddRange(cert.AuditLog());
                    }
                }
                log.Add("");

                log.Add($"Applicable tax codes are:");
                if (taxRegion.ApplicableTaxCodes != null)
                {
                    foreach (var code in taxRegion.ApplicableTaxCodes)
                    {
                        log.AddRange(code.AuditLog());
                    }
                }
                log.Add("");

                log.Add($"Available tax codes are:");
                if (taxRegion.TaxCodes != null)
                {
                    foreach (var code in taxRegion.TaxCodes)
                    {
                        log.AddRange(code.AuditLog());
                    }
                }
                log.Add("");
            }
            return log;
        }
    }

    public class EffectiveCertificateUnallocatedRegionException : Exception { }
}
