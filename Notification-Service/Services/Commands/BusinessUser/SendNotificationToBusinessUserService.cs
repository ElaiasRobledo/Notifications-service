using Google.Apis.Auth.OAuth2;
using MediatR;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Notification_Service.Models.Request;
using Notification_Service.Settings;
using RestSharp;
using System.Net.Http;
using System.Text;
using System.Text.Unicode;
using static Notification_Service.Services.Commands.BusinessUser.SendNotificationToBusinessUserServiceHandler;

namespace Notification_Service.Services.Commands.BusinessUser
{
    public class SendNotificationToBusinessUserService : IRequest<string>
    {
        public UnicastMessageRequest Model { get; set; }
    }

    public class SendNotificationToBusinessUserServiceHandler : IRequestHandler<SendNotificationToBusinessUserService, string>, ISendNotificationToBusinessUserServiceHandler
    {
        private readonly HttpClient _httpClient;
        private readonly FirebaseSettings _settings;

        public SendNotificationToBusinessUserServiceHandler(HttpClient httpClient, IOptions<FirebaseSettings> firebaseOptions)
        {
            _httpClient = httpClient;
            _settings = firebaseOptions.Value;
        }

        public async Task<string> Handle(SendNotificationToBusinessUserService request, CancellationToken cancellationToken)
        {
            try
            {
                var _fcmUrl = "https://fcm.googleapis.com/v1/projects/we-ve-web-test/messages:send";
                var credentialsPath = _settings.WebCredentialsPath;

                var credential = GoogleCredential.FromFile(credentialsPath).CreateScoped("https://www.googleapis.com/auth/firebase.messaging");

                var accessToken = await credential.UnderlyingCredential.GetAccessTokenForRequestAsync();

                var messagePayload = new
                {
                    message = new
                    {
                        token = request.Model.RegistrationToken,
                        notification = new
                        {
                            title = request.Model.Title,
                            body = request.Model.Body
                        }
                    }
                };
                var jsonRequest = System.Text.Json.JsonSerializer.Serialize(messagePayload);
                var requestMessage = new HttpRequestMessage(HttpMethod.Post, _fcmUrl)
                {
                    Headers = { Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", accessToken) },
                    Content = new StringContent(jsonRequest, encoding: Encoding.UTF8, "application/json")
                };
                var response = await _httpClient.SendAsync(requestMessage, cancellationToken);
                var responseContent = await response.Content.ReadAsStringAsync();

                if (response.IsSuccessStatusCode)
                {
                    return "Message sent successfully";
                }
                throw new Exception($"Error al enviar notificación: {responseContent}");
            }
            catch (Exception ex)
            {
                return $"Excepción al enviar notificación: {ex.Message}";
            }
        }

        public interface ISendNotificationToBusinessUserServiceHandler
        {
            Task<string> Handle(SendNotificationToBusinessUserService request, CancellationToken cancellationToken);
        }
    }
}