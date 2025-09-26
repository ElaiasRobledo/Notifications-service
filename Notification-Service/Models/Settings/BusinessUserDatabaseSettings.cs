namespace Notification_Service.Models.Settings
{
    public class BusinessUserDatabaseSettings
    {
        public string ConnectionString { get; set; } = null!;

        public string DatabaseName { get; set; } = null!;

        public string CollectionName { get; set; } = null!;

    }
}
