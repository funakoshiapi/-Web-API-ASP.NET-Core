
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System.Collections.Generic;


namespace TaxEngineMongoDbRepository.Model
{
    [BsonIgnoreExtraElements]
    public class TtrRate
    {

        [BsonId]
        [BsonElement("_id")]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        [BsonElement("shapes")]
        public List<Shape> Shapes { get; set; }

        [BsonElement("fuel")]
        public List<Fuel> Fuel { get; set; }

        [BsonElement("lat")]
        public double Lat { get; set; }

        [BsonElement("lng")]
        public double Lng { get; set; }

        [BsonElement("address")]
        public bool Address { get; set; }
    }
}
