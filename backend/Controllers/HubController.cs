using Microsoft.AspNetCore.SignalR;
using Microsoft.AspNetCore.Mvc;

namespace BiometricFaceApi.Hubs
{
    [ApiController]
    [Route("api/[controller]")]
    public class HubController : ControllerBase
    {
        private readonly IHubContext<CommunicationHub> _hubContext;

        public HubController(IHubContext<CommunicationHub> hubContext)
        {
            _hubContext = hubContext;
        }

        // Método para enviar um log ao hub
        [HttpPost("send-log")]
        public async Task<IActionResult> SendLog([FromBody] string logData)
        {
            await _hubContext.Clients.All.SendAsync("ReceiveLog", logData);
            return Ok("Log enviado com sucesso!");
        }
    }
}
