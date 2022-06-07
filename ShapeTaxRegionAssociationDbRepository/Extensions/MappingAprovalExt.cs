using MappingCommon;
using System;
using System.Collections.Generic;
using System.Text;
using TaxMappingRepository.Models;

namespace TaxMappingRepository.Extensions
{
    public static class MappingAprovalExt
    {
        public static MappingApproval ApprovalDbModel(this ApprovalInput approvalInput)
        {
            var outputApproval = new MappingApproval()
            {
                Icao = (approvalInput.Icao ?? "").Trim().ToUpper(),
                TaxRegion = (approvalInput.TaxRegionCode ?? "").Trim().ToUpper(),
                Latitude = (approvalInput.Latitude ?? "").Trim().ToUpper(),
                Longitude = (approvalInput.Longitude ?? "").Trim().ToUpper(),
                Address = (approvalInput.Address ?? "").Trim().ToUpper()
            };

            return outputApproval;
        }

    }
}


    

