using System;
using System.Collections.Generic;
using System.Linq;
using TaxEngineAPI.Entity.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace TaxEngineAPI.Entity
{
    public interface ITaxEngineRepository
    {
        TaxRegion GetTaxRegion(string taxRegionCode);
        IList<TaxRegion> GetTaxRegionChildren(string taxRegionCode, bool includeChildren = false);
        IList<TaxRegion> GetTaxRegionChain(string taxRegionCode, DateTime? txDate = null);
        ISet<TaxCode> GetItemTaxGroupTaxCode(string itemTaxGroup, DateTime txDate);
        IList<TaxRegion> ApplyTaxCertificate(
            IList<TaxRegion> taxRegionChain,
            string customerNumber,
            DateTime txDate
        );
        IList<TaxRegion> ApplyItemTaxGroup(
            IList<TaxRegion> taxRegionChain,
            string itemTaxGroup,
            DateTime txDate
        );

        IList<TaxCertificateRule> GetTaxCertificateRules(
            TaxCertificate taxCertificate,
            ISet<AttributeTag> attributeTags,
            ISet<TaxCertificate> availableDynamicTaxCertificates,
            DateTime xaDate
        );

        IList<TaxExemptionRule> GetTaxCodeExemptionRules(
            TaxCode taxCode,
            ISet<TaxCertificate> taxCertificates,
            ISet<TaxCode> allTaxCodesInRegionChain,
            DateTime xaDate
        );

        AttributeTag FindAttributeTag(string tag);
        AttributeTag FindAttributeTagByPk(string guid);
        TaxCertificate FindTaxCertificate(string certificateId);
        TaxCertificate FindTaxCertificateByPk(string guid);
        IDictionary<string, string> GetCoordinateByIcao(string codeName);
    }

    public class TaxEngineRepository : ITaxEngineRepository
    {
        private readonly ILogger<TaxEngineRepository> _logger;
        private readonly TaxEngineDbContext _db;

        public TaxEngineRepository(ILogger<TaxEngineRepository> logger, TaxEngineDbContext db)
        {
            _db = db;
            _logger = logger;
        }

        public TaxRegion GetTaxRegion(string taxRegionCode)
        {
            var taxRegion = _db.TaxRegions
                .Include(x => x.ParentRegion)
                .FirstOrDefault(
                    x =>
                        x.TaxRegionCode.ToUpper() == (taxRegionCode ?? "").Trim().ToUpper());

            return taxRegion;
        }

        public IList<TaxRegion> GetTaxRegionChildren(string taxRegionCode, bool includeChildren = false)
        {
            var parentNode = !string.IsNullOrWhiteSpace(taxRegionCode) ? GetTaxRegion(taxRegionCode) : null;

            var searchId = parentNode == null ? null : parentNode == null ? null : parentNode.Id;

            var nodes = GetTaxRegionsByParentId(searchId);

            if (includeChildren && nodes.Any())
            {
                foreach (var node in nodes)
                {
                    node.Children = GetTaxRegionsByParentId(node.Id);
                }
            }

            return nodes;
        }

        public IList<TaxRegion> GetTaxRegionsByParentId(string parentId)
        {
            IList<TaxRegion> taxRegions = null;

            var searchId = string.IsNullOrWhiteSpace(parentId) ? null : parentId;

            taxRegions = _db.TaxRegions
                .Where(x => x.ParentRegionId == searchId)
                .ToList();

            return taxRegions;
        }


        public IList<TaxRegion> GetTaxRegionChain(string taxRegionCode, DateTime? txDate)
        {
            var taxRegionChain = new Stack<TaxRegion>();

            var taxRegion = GetTaxRegion(taxRegionCode);
            while (taxRegion != null)
            {
                taxRegionChain.Push(taxRegion);
                taxRegion =
                    taxRegion.ParentRegion != null
                        ? GetTaxRegion(taxRegion.ParentRegion.TaxRegionCode)
                        : null;
            }

            var result =
                txDate != null
                    ? FilterTaxCode(taxRegionChain.ToList(), (DateTime)txDate)
                    : taxRegionChain.ToList();
            foreach (var item in result)
                item.LevelIndex = result.IndexOf(item);

            return result;
        }

        public ISet<TaxCode> GetItemTaxGroupTaxCode(string itemTaxGroup, DateTime txDate)
        {
            var taxCodeSet = new SortedSet<TaxCode>(new SimpleTaxCodeComparer());

            var query =
                from itg in _db.ItemTaxGroups
                join d in _db.ItemTaxGroupDetails on itg.Id equals d.ItemTaxGroupId
                join c in _db.TaxCodes on d.TaxCodeId equals c.Id
                where
                    itg.ItemTaxGroupCode.Trim().ToUpper() == itemTaxGroup.Trim().ToUpper()
                    && d.EffectiveFrom <= txDate
                    && d.EffectiveTo >= txDate
                orderby c.Id ,d.EffectiveTo descending
                select new { TaxCode = c, ItemTaxGroupDetail = d };

            foreach (var result in query)
                if (taxCodeSet.All(x => x.Id != result.TaxCode.Id))
                    taxCodeSet.Add(result.TaxCode);

            return taxCodeSet;
        }

        public IList<TaxRegion> ApplyTaxCertificate(
            IList<TaxRegion> taxRegionChain,
            string customerNumber,
            DateTime txDate
        )
        {
            customerNumber = customerNumber == null ? "" : customerNumber.Trim().ToUpper();

            if (taxRegionChain != null)
                foreach (var taxRegion in taxRegionChain)
                {
                    taxRegion.ApplicableCertificates.Clear();
                    var applicableCerts =
                        from ctc in _db.CustomerTaxCertificates
                        join cust in _db.CustomerMasters on ctc.CustomerId equals cust.Id
                        join cert in _db.TaxCertificates on ctc.TaxCertificateId equals cert.Id
                        where
                            ctc.ValidFrom <= txDate
                            && ctc.ValidTo >= txDate
                            && cust.CustomerNumber.Trim().ToUpper() == customerNumber
                            && cert.TaxRegionId == taxRegion.Id
                            && cert.Dynamic == false
                        orderby cert.MajorLevel ,cert.MinorLevel
                        select cert;

                    applicableCerts
                        .ToList()
                        .ForEach(
                            x =>
                            {
                                taxRegion.ApplicableCertificates.Add(x);
                            }
                        );

                    taxRegion.AvailableDynamicCertificates.Clear();
                    var availableDynamicCerts =
                        from cert in _db.TaxCertificates
                        join reg in _db.TaxRegions on cert.TaxRegionId equals reg.Id
                        where reg.Id == taxRegion.Id && cert.Dynamic == true
                        orderby cert.MajorLevel ,cert.MinorLevel
                        select cert;

                    availableDynamicCerts
                        .ToList()
                        .ForEach(
                            x =>
                            {
                                taxRegion.AvailableDynamicCertificates.Add(x);
                            }
                        );
                }

            return taxRegionChain;
        }

        public IList<TaxRegion> ApplyItemTaxGroup(
            IList<TaxRegion> taxRegionChain,
            string itemTaxGroup,
            DateTime txDate
        )
        {
            itemTaxGroup = itemTaxGroup == null ? "" : itemTaxGroup.Trim().ToUpper();

            if (taxRegionChain != null)
            {
                var itemTaxGroupCodes = GetItemTaxGroupTaxCode(itemTaxGroup, txDate);
                var taxRegionCodes = taxRegionChain.TaxCodes();

                var intersection = itemTaxGroupCodes
                    .Intersect(taxRegionCodes, new SimpleTaxCodeEqualityComparer())
                    .ToList()
                    .OrderBy(x => x.Code)
                    .ToList();

                foreach (var taxRegion in taxRegionChain)
                {
                    taxRegion.ApplicableTaxCodes.Clear();

                    taxRegion.TaxCodes
                        .Intersect(intersection, new SimpleTaxCodeEqualityComparer())
                        .ToList()
                        .ForEach(
                            x =>
                            {
                                taxRegion.ApplicableTaxCodes.Add(x);
                            }
                        );
                }
            }

            return taxRegionChain;
        }

        public IList<TaxCertificateRule> GetTaxCertificateRules(
            TaxCertificate taxCertificate,
            ISet<AttributeTag> attributeTags,
            ISet<TaxCertificate> availableDynamicTaxCertificates,
            DateTime xaDate
        )
        {
            var attributeTagIds = new List<string>();
            var availableDynamicTaxCertificateIds = new List<string>();
            var result = new List<TaxCertificateRule>();

            attributeTags
                ?.ToList()
                .ForEach(
                    x =>
                    {
                        attributeTagIds.Add(x.Id);
                    }
                );
            availableDynamicTaxCertificates
                ?.ToList()
                .ForEach(
                    x =>
                    {
                        availableDynamicTaxCertificateIds.Add(x.Id);
                    }
                );

            if (taxCertificate != null)
                result = (
                    from r in _db.TaxCertificateRules
                    join c in _db.TaxCertificates on r.CertificateId equals c.Id
                    join rc in _db.TaxCertificates on r.ResultCertificateId equals rc.Id
                    join attr in _db.AttributeTags on r.AttributeId equals attr.Id
                    where
                        r.CertificateId == taxCertificate.Id
                        && r.EffectiveFrom <= xaDate
                        && r.EffectiveTo >= xaDate
                        && attributeTagIds.Contains(r.AttributeId)
                        && availableDynamicTaxCertificateIds.Contains(r.ResultCertificateId)
                    orderby r.Priority ,attr.MajorLevel ,attr.MinorLevel ,rc.MajorLevel ,rc.MinorLevel
                    select r
                ).ToList();

            return result;
        }

        public IList<TaxExemptionRule> GetTaxCodeExemptionRules(
            TaxCode taxCode,
            ISet<TaxCertificate> taxCertificates,
            ISet<TaxCode> allTaxCodesInRegionChain,
            DateTime xaDate
        )
        {
            var result = new List<TaxExemptionRule>();
            var allTaxCodeIdsInRegionChain = new List<string>();
            var allTaxCertificateIds = new List<string>();

            if (taxCode != null)
            {
                #region setup all tax codes id in region chain

                allTaxCodesInRegionChain
                    ?.ToList()
                    .ForEach(
                        x =>
                        {
                            if (x.LevelIndex >= taxCode.LevelIndex)
                                allTaxCodeIdsInRegionChain.Add(x.Id);
                        }
                    );

                #endregion

                #region setup all tax certificate id

                taxCertificates
                    ?.ToList()
                    .ForEach(
                        x =>
                        {
                            if (x.LevelIndex <= taxCode.LevelIndex)
                                allTaxCertificateIds.Add(x.Id);
                        }
                    );

                #endregion

                result = (
                    from r in _db.TaxExemptionRules
                    join t in _db.TaxCodes on r.ResultTaxCodeId equals t.Id
                    join c in _db.TaxCertificates on r.CertificateId equals c.Id
                    where
                        r.TaxCodeId == taxCode.Id
                        && r.EffectiveFrom <= xaDate
                        && r.EffectiveTo >= xaDate
                        && allTaxCertificateIds.Contains(r.CertificateId)
                        && allTaxCodeIdsInRegionChain.Contains(r.ResultTaxCodeId)
                    orderby r.Priority ,c.MajorLevel ,c.MinorLevel ,t.MajorLevel ,t.MinorLevel
                    select r
                ).ToList();
            }

            return result;
        }

        public AttributeTag FindAttributeTag(string tag)
        {
            tag = tag == null ? "" : tag.Trim().ToUpper();

            return _db.AttributeTags
                .Where(x => x.Tag.Trim().ToUpper() == tag)
                .ToList()
                .FirstOrDefault();
        }

        public AttributeTag FindAttributeTagByPk(string guid)
        {
            return _db.AttributeTags.Find(guid.Trim().ToUpper());
        }

        public TaxCertificate FindTaxCertificate(string certificateId)
        {
            certificateId = certificateId == null ? "" : certificateId.Trim().ToUpper();

            return _db.TaxCertificates
                .Where(x => x.CertificateId.Trim().ToUpper() == certificateId)
                .ToList()
                .FirstOrDefault();
        }

        public TaxCertificate FindTaxCertificateByPk(string guid)
        {
            return _db.TaxCertificates.Find(guid == null ? "" : guid.Trim().ToUpper());
        }

        private IList<TaxRegion> FilterTaxCode(IList<TaxRegion> taxRegionChain, DateTime txDate)
        {
            if (taxRegionChain != null)
                foreach (var taxRegion in taxRegionChain)
                    _db.TaxCodeRegions
                        .Include(x => x.TaxCode)
                        .Where(
                            x =>
                                x.TaxRegionId == taxRegion.Id
                                && x.EffectiveFrom <= txDate
                                && x.EffectiveTo >= txDate
                        )
                        .ToList()
                        .ForEach(
                            x =>
                            {
                                taxRegion.TaxCodeRegions.Add(x);
                            }
                        );

            return taxRegionChain;
        }

        public IDictionary<string, string> GetCoordinateByIcao(string codeName)
        {
            var airportLocation = _db.AirportGoogleLocations
                .FirstOrDefault(
                    x =>
                        x.Icao.ToUpper() == (codeName ?? "").Trim().ToUpper());
            
            if(airportLocation != null)
            {
                return new Dictionary<string, string>()
                {
                    {"lat", airportLocation.Latitude.ToString()},
                    {"lng", airportLocation.Longitude.ToString()}
                };
            }

            return null;
        }
    }
}
