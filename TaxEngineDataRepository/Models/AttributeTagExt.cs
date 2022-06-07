using System;
using System.Collections.Generic;
using System.Text;

namespace TaxEngineAPI.Entity.Models
{
    public static class AttributeTagExtension
    {
        public static IList<string> AuditLog(this AttributeTag tag)
        {
            List<string> log = new List<string>();

            if (tag!=null)
                log.Add($"Tag {tag.Tag} [{tag.Id}], major {tag.MajorLevel}, minor {tag.MinorLevel}");

            return log;
        }

        public static IList<string> AuditLog(this ISet<AttributeTag> tags)
        {
            List<string> log = new List<string>();

            if (tags != null)
            {
                foreach (var tag in tags)
                {
                    log.AddRange(tag.AuditLog());
                }
            }

            return log;
        }
    }
}
