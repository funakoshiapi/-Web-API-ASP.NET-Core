

using MongoDB.Bson.Serialization.Attributes;

namespace TaxEngineMongoDbRepository.Model
{
    [BsonIgnoreExtraElements]
    public class Shape
    {
        
        [BsonElement("id")]
        public string ShapeId { get; set; }

        [BsonElement("timeline_group_id")]
        public string TimelineGroupId { get; set; }

        [BsonElement("name")]
        public string Name { get; set; }

        [BsonElement("rate_value")]
        public string RateValue { get; set; }

        [BsonElement("flat_rate")]
        public string FlatRate { get; set; }

        [BsonElement("notes")]
        public string Notes { get; set; }

        [BsonElement("jurisdiction_name")]
        public string JurisdictionName { get; set; }

        [BsonElement("jurisdiction_id")]
        public string JurisdictionId { get; set; }

        [BsonElement("rate_type_id")]
        public string RateTypeId { get; set; }
    }
}
