using BiometricFaceApi.Models;
using BiometricFaceApi.Repositories.Interfaces;
using BiometricFaceApi.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BiometricFaceApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MonitorEsdController : ControllerBase
    {
        private readonly MonitorEsdService _service;
        private readonly StationViewService _stationViewService;

        public MonitorEsdController(IMonitorEsdRepository monitorEsdRepository, IStationViewRepository stationViewRepository)
        {
            _service = new MonitorEsdService(monitorEsdRepository, stationViewRepository);
        }

        /// <summary>
        /// Busca todos os monitores ESD.
        /// </summary>
       [Authorize(Roles = "administrador,tecnico")]
        [HttpGet]
        [Route("todosmonitores")]
        public async Task<ActionResult> BuscarTodos()
        {
            var (result, statusCode) = await _service.GetAllMonitorEsds();
            return StatusCode(statusCode, result);
        }

        /// <summary>
        /// Busca um monitor ESD por ID.
        /// </summary>
       [Authorize(Roles = "administrador,tecnico")]
        [HttpGet]
        [Route("{id}")]
        public async Task<ActionResult> BuscarIdMonitor(int id)
        {
            var (result, statusCode) = await _service.GetMonitorId(id);
            return StatusCode(statusCode, result);
        }

        /// <summary>
        /// Busca monitor ESD por SN.
        /// </summary>
       [Authorize(Roles = "administrador,tecnico")]
        [HttpGet]
        [Route("Pesquisa/{sn}")]
        public async Task<ActionResult> BuscarMonitorBySerialNumber(string sn)
        {
            var (result, statusCode) = await _service.GetMonitorBySerial(sn);
            return StatusCode(statusCode, result);
        }

        /// <summary>
        /// Criar ou atualizar monitor ESD.
        /// </summary>
        [Authorize(Roles = "administrador,tecnico")]
        [HttpPost]
        [Route("monitores")]
        public async Task<ActionResult> ManagerMonitor([FromBody] MonitorEsdModel model)
        {
            var (result, statusCode) = await _service.Include(model);
            return StatusCode(statusCode, result);
        }

        /// <summary>
        /// Deleta monitor ESD por ID.
        /// </summary>
       [Authorize(Roles = "administrador,tecnico")]
        [HttpDelete]
        [Route("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            var (result, statusCode) = await _service.Delete(id);
            return StatusCode(statusCode, result);
        }
    }
}
