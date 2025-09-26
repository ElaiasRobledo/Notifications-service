using FirebaseAdmin.Messaging;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using Notification_Service.Models.Entities;

namespace Notification_Service.Models.MongoEntities
{
    public class DriverUser
    {
        [BsonId]
        [BsonRepresentation(BsonType.String)]
        public string _id { get; set; }
        public string DisplayName { get; set; }
        public string Name { get; set; }
        public string LastName { get; set; }
        public string PhoneNumber { get; set; }
        public string Locale { get; set; }
        public string Email { get; set; }
        public bool EmailVerified { get; set; }
        public string City { get; set; }
        public string Uid { get; set; }
        public string Country { get; set; }
        public DateTime BornDate { get; set; }
        public string UpdatedAt { get; set; }
        public DateTime CreatedOn { get; set; }
        public DateTime ModifiedOn { get; set; }
        public string photoUrl { get; set; }
        public string Picture { get; set; }
        public string RegistrationToken { get; set; }
        public List<SearchHistory> SearchHistory { get; set; }
        public List<Notification>? DriverUserNotifications { get; set; }
    }
}
