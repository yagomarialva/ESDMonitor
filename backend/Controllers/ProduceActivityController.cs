using BiometricFaceApi.Models;
using BiometricFaceApi.Repositories.Interfaces;
using BiometricFaceApi.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Net.Http.Headers;
using System.Text.Json;

namespace BiometricFaceApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProduceActivityController : Controller
    {
        private readonly ProduceActivityService _produceActivityService;
        private readonly RecordStatusService _recordStatusService;
        private readonly UserService _userService;
        private readonly JigService _jigService;
        private readonly MonitorEsdService _monitorEsdService;
        private readonly StationService _stationService;
        private readonly LineService _lineService;
        private readonly LinkStationAndLineService _linkStationAndLineService;
        private readonly StationViewService _stationViewService;
        public ProduceActivityController(IProduceActivityRepository produceActivityRepository, 
            IRecordStatusRepository recordStatusRepository, IUsersRepository usersRepository, 
            IAuthenticationRepository authenticationRepository, IJigRepository jigRepository, 
            ILinkStationAndLineRepository linkStationAndLineRepository,
            IStationViewRepository stationViewRepository,
            IMonitorEsdRepository monitorEsdRepository,
            IStationRepository stationRepository, 
            ILineRepository lineRepository)
        {
            _recordStatusService = new RecordStatusService(recordStatusRepository);
            _userService = new UserService (usersRepository);
            _jigService = new JigService (jigRepository);
            _linkStationAndLineService = new LinkStationAndLineService(linkStationAndLineRepository, stationRepository, lineRepository);
            _monitorEsdService = new MonitorEsdService (monitorEsdRepository,stationViewRepository );
            _produceActivityService = new ProduceActivityService(produceActivityRepository, recordStatusRepository, authenticationRepository,usersRepository, jigRepository, linkStationAndLineRepository, monitorEsdRepository);
        }
        /// <summary>
        /// Retorna todos os registros de produção.
        /// </summary>
       [Authorize(Roles = "administrador,tecnico")]
        [HttpGet("TodaProducao")]
        public async Task<IActionResult> GetAllProduction()
        {
            var (result, statusCode) = await _produceActivityService.GetAllProduceActivitiesAsync();
            return StatusCode(statusCode, result);
        }

        /// <summary>
        /// Busca dados de produção por ID.
        /// </summary>
       [Authorize(Roles = "administrador,tecnico")]
        [HttpGet("BuscarProducao/{id}")]
        public async Task<IActionResult> GetProductionById(int id)
        {
            var (result, statusCode) = await _produceActivityService.GetProduceActivityByIdAsync(id);
            return StatusCode(statusCode, result);
        }

        /// <summary>
        /// Criar ou atualizar dados de produção.
        /// </summary>
        [Authorize(Roles = "administrador,tecnico")]
        [HttpPost("AdicionarProducao")]
        public async Task<IActionResult> AddOrUpdateProduction([FromBody] ProduceActivityModel model)
        {
            var (result, statusCode) = await _produceActivityService.AddOrUpdateProduceActivityAsync(model);
            return StatusCode(statusCode, result);
        }

        /// <summary>
        /// Altera o status de produção.
        /// </summary>
       [Authorize(Roles = "administrador,tecnico")]
        [HttpPost("ChangeStatus")]
        public async Task<IActionResult> ChangeProductionStatus(int id, bool status, string? description)
        {
            var token = Request.Headers[HeaderNames.Authorization].ToString().Split(" ").LastOrDefault();
            var (result, statusCode) = await _produceActivityService.ChangeProduceActivityStatusAsync(id, status, description, token);
            return StatusCode(statusCode, result);
        }

        /// <summary>
        /// Deleta registro de produção por ID.
        /// </summary>
       [Authorize(Roles = "administrador,tecnico")]
        [HttpDelete("DeleteProducao/{id}")]
        public async Task<IActionResult> DeleteProduction(int id)
        {
            var (result, statusCode) = await _produceActivityService.DeleteProduceActivityAsync(id);
            return StatusCode(statusCode, result);
        }
    }
}
