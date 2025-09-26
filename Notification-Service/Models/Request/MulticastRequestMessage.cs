namespace Notification_Service.Models.Request
{
    public class MulticastRequestMessage
    {
        public string Title { get; set; }
        public string Body { get; set; }
        public List<string> RegistrationToken { get; set; }
    }
}
