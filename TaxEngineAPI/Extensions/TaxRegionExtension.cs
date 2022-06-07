using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TaxEngineAPI.Json;
using TaxEngineAPI.Entity.Models;
using Newtonsoft.Json;

namespace TaxEngineAPI.Model
{
    public static class TaxRegionExtension
    {
        public static TaxRegionNodeJson GetJson(this TaxRegion region)
        {
            if (region == null)
                return null;

            var json = new TaxRegionNodeJson()
            {
                Id = region.Id,
                Code = region.TaxRegionCode,
                Description = region.TaxRegionDescription,
                ParentId = region.ParentRegionId,
                HasChildren = region.HasChildren
            };

            return json;
        }
    }
}
