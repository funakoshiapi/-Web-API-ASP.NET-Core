using Microsoft.AspNetCore.Mvc;
using TaxEngineMongoDbRepository;
using TaxEngineMongoDbRepository.Model;
using TaxEngineAPI.Extensions;
using TaxEngineAPI.Json.TtrShape;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Linq;
using MappingCommon;
using TaxMappingRepository;
using TaxMappingRepository.Models;
using Newtonsoft.Json;
using TaxEngineAPI.Entity;
using GoogleMaps.LocationServices;
using System.Net;
using System.IO;
using TaxEngineAPI.Json;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;

namespace TaxEngineAPI.Controllers
{
    [ApiController]
    [Produces("application/json")]
    public class TtrController : ControllerBase
    {
        private readonly ITtrRepository _mongoDbRepo;
        private readonly IMappingRepository _mappingRepo;
        private readonly ITaxEngineRepository _taxEngineRepository;
        private readonly IConfiguration _config;
        private readonly ILogger<TtrController> _logger;

        public TtrController(ITaxEngineRepository taxEngineRepository, ITtrRepository repo, IMappingRepository mappingRepo, ILogger<TtrController> logger, IConfiguration config)
        {
            _mongoDbRepo = repo;
            _taxEngineRepository = taxEngineRepository;
            _mappingRepo = mappingRepo;
            _logger = logger;
            _config = config;
        }

        [Route("TtrGetGeoJsonByShapeId")]
        [HttpGet]
        public IActionResult  TtrGetGeoJsonByShapeId(string shapeId)
        {
            var geoJson = _mongoDbRepo.TtrGetGeoJsonByShapeId(shapeId);

            if (geoJson != null)
            {
                var json = JObject.Parse(geoJson);
                return Ok(json);
            }
            return NotFound();
        }

        [Route("TtrGetShapeByCoordinate")]
        [HttpGet]
        public IActionResult TtrGetShapeByCoordinate(string lat, string lng)
        {
            TtrRate rate = _mongoDbRepo.TtrGetRateByCoordinate(lat, lng);

            TtrShape ttrShape = rate.GetShapeJson();

            if (ttrShape == null)
                return NotFound();

            return Ok(ttrShape);
        }

        [Route("TtrGetShapeByIcao")]
        [HttpGet]
        public IActionResult TtrGetShapeByIcao(string codeName)
        {
            var coordinates = _taxEngineRepository.GetCoordinateByIcao(codeName);

            if (coordinates == null)
            {
                return NotFound();
            }
            return TtrGetShapeByCoordinate(coordinates["lat"], coordinates["lng"]);
        }

        [Route("TtrGetShapeRateByIcao")]
        [HttpGet]
        public IActionResult TtrGeShapeRateByIcao(string codeName)
        {
            var coordinates = _taxEngineRepository.GetCoordinateByIcao(codeName);

            if (coordinates == null)
            {
                return NotFound();
            }
            else
            {
                TtrRate rate = _mongoDbRepo.TtrGetRateByCoordinate(coordinates["lat"], coordinates["lng"]);

                List<TtrShapeRate> ttrShapeRate = rate.GetShapeRateJson();

                if (ttrShapeRate == null)
                    return NotFound();

                return Ok(ttrShapeRate);
            }
           
        }

        [Route("TtrGetShapeRateByCoordinate")]
        [HttpGet]
        public IActionResult TtrGetShapeRateByCoordinate(string lat, string lng)
        {
                TtrRate rate = _mongoDbRepo.TtrGetRateByCoordinate(lat, lng);

                List<TtrShapeRate> ttrShapeRate = rate.GetShapeRateJson();

                if (ttrShapeRate == null)
                    return NotFound();

                return Ok(ttrShapeRate);
        }

        [Route("TtrGetShapeRateById")]
        [HttpGet]
        public IActionResult TtrGetShapeRateById(string id) 
        { 
            TtrRate rate = _mongoDbRepo.TtrGetRateByShapeId(id);

            if (rate == null)
            {
                return NotFound();
            }

            List<TtrShapeRate> ttrShapeRate = rate.GetShapeRateById(id);

            if (ttrShapeRate == null)
                return NotFound();

            return Ok(ttrShapeRate);
        }



        [HttpGet("TtrGetMapping/{shapeId}", Name = "TtrGetMapping")]
        public IActionResult TtrGetMapping(string shapeId)
        {
            Mapping mapping = _mappingRepo.GetMapping(shapeId, ShapeSources.Ttr);

            if (mapping == null)
            {
                return Ok(new object());
            }
            return Ok(mapping);
        }



        [HttpGet("TtrGetApprovalMapping")]
        public IActionResult TtrGetApprovalMapping(string approvalId)
        {
            var approvalMapping = _mappingRepo.GetApprovalMappingById(approvalId);

            if (approvalMapping == null)
            {
                return Ok(new object());
            }
            return Ok(approvalMapping);
        }

        [Route("TtrGetShapeByAddress")]
        [HttpGet]
        public IActionResult TtrGetShapeByAddress(string address)
        {
            string apiKey = _config["Google:GeocodingApiKey"];
            var coordinates = GoogleGeocoding.ForwardGeocoding(address, apiKey);
            if(coordinates != null)
            {
                return TtrGetShapeByCoordinate(coordinates["lat"], coordinates["lng"]);
            }
            return NotFound();
        }


        [Route("TtrAddMapping")]
        [HttpPut]
        public IActionResult TtrAddMapping(MappingInput mapping)
        {
            _mappingRepo.AddMapping(mapping, ShapeSources.Ttr);
            var addedMapping = _mappingRepo.GetMapping(mapping.ShapeId, ShapeSources.Ttr);

            return CreatedAtRoute(nameof(TtrGetMapping), new { shapeId = addedMapping.ShapeId, source = ShapeSources.Ttr }, addedMapping);
        }


        [Route("TtrSendApprovalRequest")]
        [HttpPut]
        public IActionResult TtrSendApprovalRequest(ApprovalInput approvalRequest)
        {
            _mappingRepo.AddApprovalRequest(approvalRequest);
            
            return Ok();
        }


        [Route("TtrApproveMapping")]
        [HttpGet]
        public IActionResult TtrApproveMapping (string shapeId)
        {
            var result = _mappingRepo.ApproveMapping(shapeId);

            if (result)
            {
               return TtrGetMapping(shapeId);
            }
            
            return NotFound();
        }



    }

}
