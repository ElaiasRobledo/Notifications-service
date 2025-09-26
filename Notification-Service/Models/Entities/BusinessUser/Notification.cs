namespace Notification_Service.Models.Entities.BusinessUser
{
    public class Notification
    {
        public string _id { get; set; }
        public DateTime CreatedOn { get; set; }
        public DateTime ModifiedOn { get; set; }
        public string Title { get; set; }
        public string Body { get; set; }
        public bool status { get; set; }
        public string Action { get; set; }
    }
}
