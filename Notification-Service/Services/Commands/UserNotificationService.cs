using FirebaseAdmin;
using FirebaseAdmin.Messaging;
using Google.Apis.Auth.OAuth2;
using MediatR;
using Microsoft.Extensions.Options;
using Notification_Service.Models.Request;
using Notification_Service.Settings;
using static Notification_Service.Services.Commands.UserNotificationServiceHandler;

namespace Notification_Service.Services.Commands
{
    public class UserNotificationService : IRequest<string>
    {
        public UnicastMessageRequest Model { get; set; }
    }

    public class UserNotificationServiceHandler : IRequestHandler<UserNotificationService, string>, IUserNotificationServiceHandler
    {
        private readonly FirebaseSettings _firebaseSettings;

        public UserNotificationServiceHandler(IOptions<FirebaseSettings> firebaseSettings)
        {
            _firebaseSettings = firebaseSettings.Value;
        }

        public async Task<string> Handle(UserNotificationService request, CancellationToken cancellationToken)
        {
            try
            {
                var message = new Message()
                {
                    Notification = new Notification
                    {
                        Title = request.Model.Title,

                        Body = request.Model.Body,
                    },

                    Token = request.Model.RegistrationToken,
                };

                var messaging = FirebaseMessaging.DefaultInstance;
                var result = await messaging.SendAsync(message);

                if (!string.IsNullOrEmpty(result))
                {
                    return "Message sent successfully!";
                }
                else
                {
                    throw new Exception("Error sending the message.");
                }
            }
            catch (FirebaseMessagingException ex)
            {
                throw new Exception($"Firebase Error: {ex.Message}"); 
            }
            catch (Exception ex)
            {
                return $"An unexpected error occurred while sending the notification: {ex.Message}";
            }
        }

        public interface IUserNotificationServiceHandler
        {
            Task<string> Handle(UserNotificationService request, CancellationToken cancellationToken);
        }
    }
}