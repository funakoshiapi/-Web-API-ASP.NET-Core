using MappingCommon;

using Microsoft.Extensions.Logging;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TaxMappingRepository.Extensions;
using TaxMappingRepository.Models;

namespace TaxMappingRepository
{
    public interface IMappingRepository
    {
        Mapping GetMapping(string shapeId, string source);
        void AddMapping(MappingInput mapping, string source);
        bool ApproveMapping(string shapeId);
        void AddApprovalRequest(ApprovalInput approvalRequest);
        MappingApproval GetApprovalMappingById(string id);
    }

    public class MappingRepository : IMappingRepository
    {
        private readonly MappingDataContext _db;
        private readonly ILogger<MappingDataContext> _logger;

        public MappingRepository(MappingDataContext db, ILogger<MappingDataContext> logger)
        {
            _db = db;
            _logger = logger;
        }

        public void AddMapping(MappingInput mapping, string source)
        {
            _logger.LogInformation($"Called AddMapping");

            if (mapping == null)
            {
                _logger.LogInformation($"{nameof(mapping)}");
                throw new ArgumentException(nameof(mapping));
            }
            try
            {
                _db.Mappings.Add(mapping.MapToDbModel(source));
                _db.SaveChanges();
                _logger.LogInformation($"mapping was added to db context");
            }
            catch (Exception e)
            {
                _logger.LogInformation($"mapping was not added to db context");
                _logger.LogError(e.ToString());
            }
        }

        public void AddApprovalRequest(ApprovalInput approvalRequest) 
        {
            _logger.LogInformation($"Called AddApprovalRequest");

            if (approvalRequest == null)
            {
                _logger.LogInformation($"{nameof(approvalRequest)}");
                throw new ArgumentException(nameof(approvalRequest));
            }
            try
            {
                _db.MappingApprovals.Add(approvalRequest.ApprovalDbModel());
                _db.SaveChanges();
                _logger.LogInformation($"Approval request was added to db context");
            }
            catch (Exception e)
            {
                _logger.LogInformation($"Approval request was not added to db context");
                _logger.LogError(e.ToString());
            }
        }



        public bool ApproveMapping(string shapeId)
        {
            if (string.IsNullOrEmpty(shapeId))
            {
                _logger.LogInformation($"{nameof(shapeId)}");
                return false;
            }

            var dbMapping = _db.Mappings.FirstOrDefault(m => m.ShapeId.Equals(shapeId));

            if (dbMapping == null)
                return false;

            dbMapping.Status = "Approved";
            _db.SaveChanges();
            _logger.LogInformation($"Approved mapping was added to db context");
            return true;
        }


        public MappingApproval GetApprovalMappingById(string id)
        {
            if (!string.IsNullOrEmpty(id))
            {
                return _db.MappingApprovals.FirstOrDefault(m => m.Id.Equals(id));  
            }

            return null;

        }

        public Mapping GetMapping(string shapeId, string source)
        {
            _logger.LogInformation(
                $"Called GetAssociation with shape id = {shapeId} and source = {source}"
            );

            var mapping = _db.Mappings.FirstOrDefault(
                x =>
                    x.ShapeId.Trim() == (shapeId ?? "").Trim()
                    && x.Source == (source ?? "").Trim().ToUpper()
            );

            if (mapping != null)
            {
                mapping.ShapeId = mapping.ShapeId.Trim();
                _logger.LogInformation($"mapping exists");
            }
            else
            {
                _logger.LogInformation($"mapping does not exist");
            }

            return mapping;
        }
    }
}
