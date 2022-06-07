using TaxEngineAPI.Model;
using TaxEngineAPI.Json;
using TaxEngineAPI.Entity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Linq;

namespace TaxEngineAPI.Controllers
{
    [ApiController]
    [Produces("application/json")]
    public class TaxRegionController : ControllerBase
    {
        private readonly ITaxEngineRepository _repo;
        private readonly ILogger<TaxRegionController> _logger;

        public TaxRegionController(
            ITaxEngineRepository taxEngineRepo,
            ILogger<TaxRegionController> logger
        )
        {
            _repo = taxEngineRepo;
            _logger = logger;
        }

        [Route("GetTaxRegionChildren")]
        [HttpGet]
        public ActionResult<TaxRegionNodeJson> GetTaxRegionChildren(
            [FromQuery] TaxRegionParams paramsData
        )
        {
            _logger.LogInformation(
                $"Running GetTaxRegionChildren with parameter Code = {paramsData.Code} "
                    + $"and paremeter ApiKey = {paramsData.ApiKey} "
            );

            var taxRegionJson = new List<TaxRegionNodeJson>();

            _repo
                .GetTaxRegionChildren(paramsData.Code, true)
                .ToList()
                .ForEach(
                    x =>
                    {
                        if (x != null)
                            taxRegionJson.Add(x.GetJson());
                    }
                );

            if (taxRegionJson.Count > 0)
                return Ok(taxRegionJson);

            return NotFound();
        }

        [Route("GetTaxRegionChain")]
        [HttpGet]
        public IActionResult GetTaxRegionChain(string taxRegionCode)
        {
            var taxRegionChain = new List<TaxRegionNodeJson>();
            _repo
                .GetTaxRegionChain(taxRegionCode)
                .ToList()
                .ForEach(
                    taxRegion =>
                    {
                        if (taxRegion != null)
                            taxRegionChain.Add(taxRegion.GetJson());
                    }
                );

            if (taxRegionChain.Count > 0)
            {
                return Ok(taxRegionChain);
            }
            return NotFound();
        }
    }
}
