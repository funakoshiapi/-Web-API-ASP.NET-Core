

using MongoDB.Bson.Serialization.Attributes;

namespace TaxEngineMongoDbRepository.Model
{
    [BsonIgnoreExtraElements]
    public class Fuel
    {
        [BsonElement("shape_id")]
        public string ShapeId { get; set; }

        [BsonElement("jurisdiction")]
        public string Jurisdiction { get; set; }

        [BsonElement("tax_type")]
        public string TaxType { get; set; }

        [BsonElement("fuel_type")]
        public string FuelType { get; set; }

        [BsonElement("rate")]
        public string Rate { get; set; }

        [BsonElement("effective_date")]
        public string EffectiveDate { get; set; }

        [BsonElement("end_date")]
        public string EndDate { get; set; }

        [BsonElement("notes")]
        public string Notes { get; set; }

        [BsonElement("update_frequency")]
        public string UpdateFrequency { get; set; }

        [BsonElement("citations")]
        public string Citations { get; set; }
    }
}
