namespace Notification_Service.Models.Request
{
    public class UnicastMessageRequest
    {
        public string Title { get; set; }
        public string Body { get; set; }
        public string RegistrationToken { get; set; }
    }
}
