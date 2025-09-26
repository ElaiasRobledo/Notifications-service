namespace Notification_Service.Models.Entities.BusinessUser
{
    public class BusinessUser
    {
        public string _id { get; set; }
        public DateTime CreatedOn { get; set; }
        public DateTime ModifiedOn { get; set; }
        public string Email { get; set; }
        public string Uid { get; set; }
        public string PhoneNumber { get; set; }
        public string BusinessName { get; set; }
        public string photoUrl { get; set; }
        public string Cuit { get; set; }
        public string BusinessAddress { get; set; }
        public string status { get; set; }
        public string Rubro { get; set; }
        public string? RegistrationToken { get; set; }
        public List<Notification> UserNotifications { get; set; }
    }
}
