using MongoDB.Bson.Serialization.Attributes;

namespace Notification_Service.Models.Entities
{
    [BsonIgnoreExtraElements]
    public class SearchHistory
    {
        public string Title { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public Prediction Prediction { get; set; }
    }
}
