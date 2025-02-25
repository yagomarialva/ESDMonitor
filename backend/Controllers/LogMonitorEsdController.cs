using BiometricFaceApi.Hubs;
using BiometricFaceApi.Models;
using BiometricFaceApi.Repositories.Interfaces;
using BiometricFaceApi.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;

namespace BiometricFaceApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LogMonitorEsdController : ControllerBase
    {
        private readonly LogMonitorEsdService _logMonitorEsdService;
        private readonly IMonitorEsdRepository _monitorEsdRepository;
        private readonly LastLogMonitorEsdModel _lastLogMonitorEsdModel;
        public LogMonitorEsdController(ILogMonitorEsdRepository logMonitorEsdRepository,IMonitorEsdRepository monitor,ILastLogMonitorEsdRepository lastLogMonitorEsd, IHubContext<CommunicationHub> _hubContext)
        {
          
            _logMonitorEsdService = new LogMonitorEsdService(logMonitorEsdRepository,lastLogMonitorEsd,monitor, _hubContext);
            
        }

        /// <summary>
        /// Busca monitor ESD por ID.
        /// </summary>
        [Authorize(Roles = "administrador,tecnico")]
        [HttpGet]
        [Route("ListMonitorEsd")]
        public async Task<ActionResult> BuscarListaMonitorEsdById([FromQuery] int id, [FromQuery] int page = 1, [FromQuery] int pageSize = 50)
        {
            var (result, statusCode) = await _logMonitorEsdService.GetListMonitorEsdByIdAsync(id, page, pageSize);
            return StatusCode(statusCode, result);
        }

        /// <summary>
        /// Listar em ordem crescente.
        /// </summary>
        [Authorize(Roles = "administrador,tecnico")]
        [HttpGet]
        [Route("ListLogsOrdemCrescente")]
        public async Task<ActionResult> ListaLogOrdemCrescente([FromQuery] int serialNumberEsp, [FromQuery] int limit = 10)
        {
            var (result, statusCode) = await _logMonitorEsdService.GetLogIncreAsync(serialNumberEsp, limit);
            return StatusCode(statusCode, result);
        }

        /// <summary>
        /// Listar em ordem descresente.
        /// </summary>
        [Authorize(Roles = "administrador,tecnico")]
        [HttpGet]
        [Route("ListLogsOrdemDecrescente")]
        public async Task<ActionResult> ListaLogOrdemDecrescente([FromQuery] int serialNumberEsp, [FromQuery] int limit = 10)
        {
            var (result, statusCode) = await _logMonitorEsdService.GetLogDecreAsync(serialNumberEsp, limit);
            return StatusCode(statusCode, result);
        }

        /// <summary>
        /// Busca log monitor ESD por ID.
        /// </summary>
        [Authorize(Roles = "administrador,tecnico")]
        [HttpGet]
        [Route("LOGBYID/{id}")]
        public async Task<ActionResult> BuscarLogMonitorById(int id)
        {
            var (result, statusCode) = await _logMonitorEsdService.GetByIdAsync(id);
            return StatusCode(statusCode, result);
        }

        /// <summary>
        /// Busca tipo de log de monitor ESD.
        /// </summary>
        [Authorize(Roles = "administrador,tecnico")]
        [HttpGet]
        [Route("TYPE/{type}")]
        public async Task<ActionResult> BuscarMessageType(string type)
        {
            var (result, statusCode) = await _logMonitorEsdService.GetMessageTypeAsync(type);
            return StatusCode(statusCode, result);
        }

        /// <summary>
        /// Busca tipo de conteudo log de monitor ESD.
        /// </summary>
        [Authorize(Roles = "administrador,tecnico")]
        [HttpGet]
        [Route("CONTENT/{content}")]
        public async Task<ActionResult> BuscarMessageContent(string content)
        {
            var (result, statusCode) = await _logMonitorEsdService.GetMessageContentAsync(content);
            return StatusCode(statusCode, result);
        }

        /// <summary>
        /// Busca monitor ESD por ID.
        /// </summary>
        [Authorize(Roles = "administrador,tecnico")]
        [HttpGet]
        [Route("ID/{id}")]
        public async Task<ActionResult> BuscarMonitorEsdById(int id)
        {
            var (result, statusCode) = await _logMonitorEsdService.GetMonitorEsdByIdAsync(id);
            return StatusCode(statusCode, result);
        }
        
        /// <summary>
        /// Busca monitor ESD por IP.
        /// </summary>
        [Authorize(Roles = "administrador,tecnico")]
        [HttpGet]
        [Route("IP/{ip}")]
        public async Task<ActionResult> BuscarMonitorEsdByIP(string ip)
        {
            var (result, statusCode) = await _logMonitorEsdService.GetMonitorEsdByIPAsync(ip);
            return StatusCode(statusCode, result);
        }

        /// <summary>
        /// Busca monitor ESD por SN.
        /// </summary>
        [Authorize(Roles = "administrador,tecnico")]
        [HttpGet]
        [Route("sn/{serialNumber}")]
        public async Task<ActionResult> BuscarMonitorEsdBySn(string sn)
        {
            var (result, statusCode) = await _logMonitorEsdService.GetMonitorEsdBySnAsync(sn);
            return StatusCode(statusCode, result);
        }

        /// <summary>
        /// Criar ou atualizar log.
        /// </summary>
        [HttpPost]
        [HttpPost]
        [Route("ManagerLogs")]
        public async Task<ActionResult> ManagerLogsMonitorEsd([FromBody] LogMonitorEsdModel models)
        {
            var (result, statusCode) = await _logMonitorEsdService.AddOrUpdateAsync(models);
            return StatusCode(statusCode, result);
        }


        /// <summary>
        /// Altera dados de log.
        /// </summary>
        [Authorize(Roles = "administrador,tecnico")]
        [HttpPost]
        [Route("changeLogs")]
        public async Task<IActionResult> ChangesLogsMonitorEsd(int id, bool changeLogs, string? description)
        {
            var (result, statusCode) = await _logMonitorEsdService.ChangeStatusLog(id, changeLogs, description);
            return StatusCode(statusCode, result);
        }

        /// <summary>
        /// Deleta log de monitor ESD pelo ID.
        /// </summary>
        [Authorize(Roles = "administrador,tecnico")]
        [HttpDelete]
        [Route("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            var (result, statusCode) = await _logMonitorEsdService.Delete(id);
            return StatusCode(statusCode, result);
        }

    }
}