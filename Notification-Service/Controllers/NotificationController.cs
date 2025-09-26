using FirebaseAdmin.Messaging;
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Nest;
using Notification_Service.Models.Request;
using Notification_Service.Services;
using Notification_Service.Services.Commands;
using Notification_Service.Services.Commands.BusinessUser;
using Notification_Service.Services.Queries;

namespace Notification_Service.Controllers
{
    [Route("api/v1/[controller]")]

    [ApiController]
    
    public class NotificationController : ControllerBase
    {
       private readonly IMediator _mediator;
        public NotificationController(IMediator mediator) 
        {
            _mediator = mediator;
        }

        [HttpGet("driverusertokens")]
        public async Task<IActionResult> GetDriverUserTokens()
        {
            var result = await _mediator.Send(new GetDriverUserTokens { });
            return Ok(result);
        }
        [HttpGet("businessusertokens")]
        public async Task<IActionResult> GetBusinessUserTokens()
        {
            var result = await _mediator.Send(new GetBusinessUserTokens { });
            return Ok(result);
        }
        [HttpPost("driverusers/user")]
        public async Task<IActionResult> SendUnicastMessageAsync([FromBody] UnicastMessageRequest request)
        {
            var result = await _mediator.Send(new UserNotificationService { Model = request });
            return Ok(result);
        }

        [HttpPost("driverusers/all")]
        public async Task<IActionResult> SendBroadcastMessageAsync([FromBody] BroadcastMessageRequest request)
        {
            var result = await _mediator.Send(new BroadcastNotificationService { Model = request });
            return Ok(result);
        }
        [HttpPost("driverusers/group")]
        public async Task<IActionResult> SendBroadcastMessageAsync([FromBody] MulticastRequestMessage request)
        {
            var result = await _mediator.Send(new MulticastNotificationService { Model = request });
            return Ok(result);
        }
        [HttpPost("businessusers/user")]
        public async Task<IActionResult> SendNotificationToBusinessUser([FromBody] UnicastMessageRequest request)
        {
            var result = await _mediator.Send(new SendNotificationToBusinessUserService { Model = request});
            return Ok(result);
        }
    }
}
