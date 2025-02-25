using BiometricFaceApi.Models;
using BiometricFaceApi.Repositories.Interfaces;
using BiometricFaceApi.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BiometricFaceApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StationViewController : ControllerBase
    {
        private readonly StationViewService _stationViewService;

        public StationViewController(
            IStationViewRepository stationViewRepository,
            IMonitorEsdRepository monitorEsdRepository,
            ILinkStationAndLineRepository linkStationAndLineRepository,
            IStationRepository stationRepository,
            ILineRepository lineRepository,
            ILogMonitorEsdRepository logMonitorEsdRepository,
            ILastLogMonitorEsdRepository lastLogMonitorEsdRepository)
        {
            _stationViewService = new StationViewService(
                stationViewRepository,
                monitorEsdRepository,
                linkStationAndLineRepository,
                lineRepository,
                stationRepository,
                logMonitorEsdRepository,
                lastLogMonitorEsdRepository);
        }

        /// <summary>
        /// Buscar todos as Estações View.
        /// </summary>
        /// <response code="200">Retorna todos.</response>
        /// <response code="404">Nenhuma estação encontrada.</response>
        /// <response code="500">Erro do servidor interno!</response>
       [Authorize(Roles = "administrador,tecnico")]
        [HttpGet("todasEstacaoView")]
        public async Task<ActionResult> BuscarTodos()
        {
            var (result, statusCode) = await _stationViewService.GetAllStationView();
            return StatusCode(statusCode, result);
        }

        /// <summary>
        /// Buscar Estacao View por Id.
        /// </summary>
        /// <param name="id">Id da Estação View.</param>
        /// <response code="200">Retorna Estação View por Id.</response>
        /// <response code="404">Estação View não encontrada.</response>
        /// <response code="500">Erro do servidor interno!</response>
       [Authorize(Roles = "administrador,tecnico")]
        [HttpGet("BuscarEstacaoView/{id}")]
        public async Task<ActionResult> BuscarIdEstacaoView(int id)
        {
            var (result, statusCode) = await _stationViewService.GetStationViewById(id);
            return StatusCode(statusCode, result);
        }

        /// <summary>
        /// Cadastra ou Atualiza dados do Estação View.
        /// </summary>
        /// <param name="model">Dados de cadastro da Estação View.</param>
        /// <response code="200">Dados atualizados com sucesso.</response>
        /// <response code="201">Dados cadastrados com sucesso.</response>
        /// <response code="400">Dados incorretos ou inválidos.</response>
        /// <response code="500">Erro do servidor interno!</response>
       [Authorize(Roles = "administrador,tecnico")]
        [HttpPost("adicionarEstacaoView")]
        public async Task<ActionResult> Include(StationViewModel model)
        {
            var (result, statusCode) = await _stationViewService.Include(model);
            return StatusCode(statusCode, result);
        }

        /// <summary>
        /// Gera um mapa de fábricas.
        /// </summary>
        /// <response code="200">Mapa gerado com sucesso.</response>
        /// <response code="500">Erro do servidor interno!</response>
        [HttpGet("factoryMap")]
        public async Task<ActionResult> FactoryMap()
        {
            var (result, statusCode) = await _stationViewService.FactoryView();
            return StatusCode(statusCode, result);
        }

        /// <summary>
        /// Deletar Estação View.
        /// </summary>
        /// <param name="id">Id da Estação View a ser deletada.</param>
        /// <response code="200">Dados removidos do banco de dados.</response>
        /// <response code="404">Estação View não encontrada.</response>
        /// <response code="500">Erro do servidor interno!</response>
       [Authorize(Roles = "administrador,tecnico")]
        [HttpDelete("deleteStationView/{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            var (result, statusCode) = await _stationViewService.Delete(id);
            return StatusCode(statusCode, result);
        }
    }
}
