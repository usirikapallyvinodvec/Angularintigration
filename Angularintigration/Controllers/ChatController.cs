using Angularintigration.Hubs;
using Angularintigration.Models;
using Angularintigration.Servicepattern.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Angularintigration.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ChatController : ControllerBase
    {
        private readonly IChatServices _service;

        public ChatController(
            IChatServices service)
        {
            _service = service;
        }

        [HttpGet("users/{userId}")]
        public async Task<IActionResult>
        GetUsers(int userId)
        {
            var result =
                await _service.GetUsers(
                    userId);

            return Ok(result);
        }

        
        [HttpGet("history")]
        public async Task<IActionResult>
        GetChatHistory(
            int senderId,
            int receiverId)
        {
            var result =
                await _service.GetChatHistory(
                    senderId,
                    receiverId);

            return Ok(result);
        }
        [HttpPost("send")]
        public async Task<IActionResult>
        SaveMessage(
            [FromBody]
            ChatMessageModel model)
        {
            if (model == null)
            {
                return BadRequest(
                    "Invalid Data");
            }

            var result =
                await _service.SaveMessage(
                    model);

            return Ok(new
            {
                message = "Message Sent",
                rows = result
            });
        }

  
        [HttpGet("onlineusers")]
        public IActionResult
        GetOnlineUsers()
        {
            var users =
                ChatHub.GetOnlineUsers();

            return Ok(users);
        }
    }
}