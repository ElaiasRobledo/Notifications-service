using FirebaseAdmin.Messaging;
using MediatR;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using Notification_Service.Models.MongoEntities;
using Notification_Service.Models.Request;
using Notification_Service.Models.Services;
using Notification_Service.Services.Commands;
using Notification_Service.Settings;

namespace Notification_Service.Services.Commands
{
    public class MulticastNotificationService : IRequest<string>
    {
        public MulticastRequestMessage Model { get; set; }

    }
    public class MulticastNotificationServiceHandler : IRequestHandler<MulticastNotificationService, string>, IMulticastNotificationServiceHandler
    {
        private readonly IMongoCollection<DriverUser> _DriverUserCollection;
        private readonly FirebaseSettings _firebaseSettings;

        public MulticastNotificationServiceHandler(
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

    
       public async Task<string> Handle(MulticastNotificationService request, CancellationToken cancellationToken)
        {
            try
            {

                var message = new MulticastMessage()
                {
                    Notification = new Notification
                    {
                        Title = request.Model.Title,

                        Body = request.Model.Body,
                    },
                    Tokens = request.Model.RegistrationToken,
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
 }
public interface IMulticastNotificationServiceHandler
{
    Task<string> Handle(MulticastNotificationService request, CancellationToken cancellationToken);
}