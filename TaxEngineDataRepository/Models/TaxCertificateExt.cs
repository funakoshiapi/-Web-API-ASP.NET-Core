using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace TaxEngineAPI.Entity.Models
{
    public partial class TaxCertificate
    {
        [NotMapped]
        public int LevelIndex { get; set; }

        [NotMapped]
        public bool Remove { get; set; }
        
        [NotMapped]
        public bool Final { get; set; }

        [NotMapped]
        public IList<TaxCertificate> TagTaxCertificates { get; set; } = new List<TaxCertificate>();
    }

    public static class TaxCertificateExtension
    {
        public static IList<string> AuditLog(this TaxCertificate cert)
        {
            List<string> log = new List<string>();

            if (cert != null)
            {
                log.Add($"Certificate {cert.CertificateId} [{cert.Id}], level {cert.LevelIndex}, major {cert.MajorLevel}, minor {cert.MinorLevel}");
            }

            return log;
        }

        public static IList<string> AuditLog(this ISet<TaxCertificate> certs)
        {
            List<string> log = new List<string>();

            if (certs != null)
            {
                foreach (var cert in certs)
                {
                    log.AddRange(cert.AuditLog());
                }
            }

            return log;
        }

    }
}
