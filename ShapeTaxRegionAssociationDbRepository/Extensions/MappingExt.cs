
using MappingCommon;
using System;
using TaxMappingRepository.Models;

namespace TaxMappingRepository.Extensions
{
    public static class MappingExtension
    {
        public static Mapping MapToDbModel(this MappingInput inputAssociation, string source)
        {
            var outputAssociation = new Mapping()
            {
                ShapeId = inputAssociation.ShapeId.Trim(),
                TaxRegionId = inputAssociation.TaxRegionId,
                Status = MappingStatus.Pending,
                Source = source.Trim()
            };

            return outputAssociation;
            
        }
    }
}
