using FirebaseAdmin;
using FirebaseAdmin.Messaging;
using Google.Apis.Auth.OAuth2;
using MediatR;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using Notification_Service.Models.MongoEntities;
using Notification_Service.Models.Request;
using Notification_Service.Models.Services;
using Notification_Service.Settings;

namespace Notification_Service.Services.Commands
{
    public class BroadcastNotificationService : IRequest<string>
    {
        public BroadcastMessageRequest Model { get; set; }
    }

    public class BroadcastNotificationServiceHandler : IRequestHandler<BroadcastNotificationService, string>, IBroadcastNotificationServiceHandler
    {
        private readonly IMongoCollection<DriverUser> _DriverUserCollection;
        private readonly FirebaseSettings _firebaseSettings;

        public BroadcastNotificationServiceHandler(
        IOptions<DriverUserDatabaseSettings> DriverStoreDatabaseSettings, IOptions<FirebaseSettings> firebaseOptions)
        {
            _firebaseSettings = firebaseOptions.Value;
            var mongoClient = new MongoClient(
                DriverStoreDatabaseSettings.Value.ConnectionString);

            var mongoDatabase = mongoClient.GetDatabase(
                DriverStoreDatabaseSettings.Value.DatabaseName);

            _DriverUserCollection = mongoDatabase.GetCollection<DriverUser>(
                DriverStoreDatabaseSettings.Value.CollectionName);
        }

        public async Task<string> Handle(BroadcastNotificationService request, CancellationToken cancellationToken)
        {
            try
            {
                var totalCount = await _DriverUserCollection.CountDocumentsAsync(FilterDefinition<DriverUser>.Empty);
                Console.WriteLine($"Total de documentos en la colección: {totalCount}");

                var filter = Builders<DriverUser>.Filter.Ne(c => c.RegistrationToken, null);
                var driversCursor = await _DriverUserCollection.FindAsync(filter);
                var drivers = await driversCursor.ToListAsync();

                var tokens = drivers.Select(s => s.RegistrationToken).ToList();

          
                var message = new MulticastMessage()
                {
                    Notification = new Notification
                    {
                        Title = request.Model.Title,

                        Body = request.Model.Body,
                    },
                    Tokens = tokens,
                };


                var messaging = FirebaseMessaging.DefaultInstance;
                var result = await messaging.SendEachForMulticastAsync(message);
                return "Message sent successfully!";
            }
            catch (FirebaseMessagingException ex)
            {
                return $"Firebase Error: {ex.Message}";
            }
            catch (Exception ex)
            {
                return $"An unexpected error occurred while sending the notification: {ex.Message}";
            }
        }
    }

    public interface IBroadcastNotificationServiceHandler
    {
        Task<string> Handle(BroadcastNotificationService request, CancellationToken cancellationToken);
    }
}