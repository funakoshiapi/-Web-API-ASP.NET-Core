using TaxEngineAPI.Extensions;
using TaxEngineAPI.Json;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TaxEngineAPI.Controllers
{
    [ApiController]
    [Produces("application/json")]
    public class GeoCodingController: ControllerBase
    {
        private readonly ILogger<GeoCodingController> _logger;
        private readonly IConfiguration _config;

        public GeoCodingController(ILogger<GeoCodingController> logger, IConfiguration config)
        {
            _logger = logger;
            _config = config;
        }

        [Route("GetCoordinateFromAddress")]
        [HttpGet]
        public IActionResult GetCoordinateFromAddress(string address)
        {
            var apiKey = _config["Google:GeocodingApiKey"];
            var coordinates = GoogleGeocoding.ForwardGeocoding(address, apiKey);

            if (coordinates != null)
            {
                return Ok(new Coordinate() { lat = coordinates["lat"], lng = coordinates["lng"] });
            }
            return NotFound();

        }
    }
}
