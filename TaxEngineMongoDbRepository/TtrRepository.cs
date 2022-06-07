using TaxEngineMongoDbCommon;
using TaxEngineMongoDbRepository.Model;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Driver;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;

namespace TaxEngineMongoDbRepository
{
    public interface ITtrRepository
    {
        string TtrGetGeoJsonByShapeId(string id);
        TtrRate TtrGetRateByCoordinate(string lat, string lng);
        List<TtrSearchOption> TtrGetCordinatesByStateName(string stateName);
        List<string> TtrGetStates();
        TtrRate TtrGetRateByShapeId(string id);
    }

    public class TtrRepository : ITtrRepository
    {
        private readonly IConfiguration _configuration;
        private readonly IMongoClient _mongoClient;
        private readonly IMongoDatabase _mongoDatabase;
        private readonly IMongoCollection<BsonDocument> _shapeCollection;
        private readonly IMongoCollection<BsonDocument> _rateCollection;
        private readonly ILogger<TtrRepository> _logger;

        public TtrRepository(ILogger<TtrRepository> logger, IConfiguration configuration)
        {
            _configuration = configuration;
            _logger = logger;
            _logger.LogInformation("Setting up MongoDb");

            string connectionStringSetting =
                _configuration.GetSection("MongoDB").GetSection("ConnectionString").Value;
            string dataBaseNameSetting =
                _configuration.GetSection("MongoDB").GetSection("DatabaseName").Value;
            string shapeCollectionSetting =
                _configuration.GetSection("MongoDB").GetSection("ShapeCollection").Value;
            string rateCollectionSetting =
                _configuration.GetSection("MongoDB").GetSection("RateCollection").Value;

            _mongoClient = new MongoClient(connectionStringSetting);
            _mongoDatabase = _mongoClient.GetDatabase(dataBaseNameSetting);
            _shapeCollection = _mongoDatabase.GetCollection<BsonDocument>(shapeCollectionSetting);
            _rateCollection = _mongoDatabase.GetCollection<BsonDocument>(rateCollectionSetting);
        }

        public string TtrGetGeoJsonByShapeId(string id)
        {
            _logger.LogInformation($"Called GetPolygonById with Id = {id}");

          
            var project = Builders<BsonDocument>.Projection.Exclude("_id");
            var geoJson = _shapeCollection
                .Find(new BsonDocument("properties.id", id ?? ""))
                .Project(project)
                .FirstOrDefault();

            return geoJson == null ? null : geoJson.ToJson();
        }

        public TtrRate TtrGetRateByCoordinate(string lat, string lng)
        {
            _logger.LogInformation(
                $"Called GetRateByCoordinate with latitude = {lat} and longitude = {lng}"
            );

            double latValue;
            double lngValue;

            bool validLat = double.TryParse(lat, out latValue);
            bool validLng = double.TryParse(lng, out lngValue);

            if (validLat && validLng)
            {
                var rate = _rateCollection
                    .Find(new BsonDocument { { "lat", latValue }, { "lng", lngValue } })
                    .FirstOrDefault();

                return rate == null ? null : BsonSerializer.Deserialize<TtrRate>(rate);
            }

            return null;
        }

        public TtrRate TtrGetRateByShapeId(string id)
        {
            _logger.LogInformation(
                $"Called TtrGetRateByShapeId with shapeId = {id}"
            );

                var rate = _rateCollection
                    .Find(new BsonDocument { { "shapes.id", id ?? ""} })
                    .FirstOrDefault();

                return rate == null ? null : BsonSerializer.Deserialize<TtrRate>(rate);
            
        }

        public List<TtrSearchOption> TtrGetCordinatesByStateName(string stateName)
        {
            List<TtrSearchOption> optionsList = new List<TtrSearchOption>();

            foreach (var ele in _rateCollection.Find(new BsonDocument()).ToList())
            {
                if (ele != null)
                {
                    var ttrStateName = ele.GetValue(("shapes")).AsBsonArray[0].AsBsonDocument.GetValue("name").AsString;

                    if (ttrStateName.ToUpper() == (stateName ?? "").ToUpper())
                    {
                        var option = new TtrSearchOption()
                        {
                            StateName = ttrStateName,
                            Latitude = ele.GetValue(("lat")).ToDecimal().ToString(),
                            Longitude = ele.GetValue(("lng")).ToDecimal().ToString(),
                        };

                        optionsList.Add(option);
                    }
                }
            }
            return optionsList;
        }

        public List<string> TtrGetStates()
        {
            List<string> states = new List<string>();

            foreach (var ele in _rateCollection.Find(new BsonDocument()).ToList())
            {
                if (ele != null)
                {
                    var ttrStateName = ele.GetValue(("shapes")).AsBsonArray[0].AsBsonDocument.GetValue("name").AsString;
                    if(!states.Contains(ttrStateName))
                        states.Add(ttrStateName);
                }
            }
            return states;
        }
    }
}
