using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace TaxEngineAPI.Entity.Models
{
    public partial class TaxCode
    {
        [NotMapped]
        public int LevelIndex { get; set; }

        [NotMapped] 
        public bool Final { get; set; } = false;

        [NotMapped]
        public IList<TaxCode> TagTaxCodes { get; set; } = new List<TaxCode>();
    }

    public static class TaxCodeExtension
    {
        public static IList<string> AuditLog(this TaxCode code)
        {
            List<string> log = new List<string>();

            if (code != null)
            {
                log.Add($"Tax code {code.Code} [{code.Id}], level {code.LevelIndex}, major {code.MajorLevel}, minor {code.MinorLevel}");
            }

            return log;
        }

        public static IList<string> AuditLog(this ISet<TaxCode> codes)
        {
            List<string> log = new List<string>();

            if (codes != null)
            {
                foreach (var code in codes)
                {
                    log.AddRange(code.AuditLog());
                }
            }

            return log;
        }
    }
}
