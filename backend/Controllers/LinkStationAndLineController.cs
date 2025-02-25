using BiometricFaceApi.Models;
using BiometricFaceApi.Repositories.Interfaces;
using BiometricFaceApi.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BiometricFaceApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LinkStationAndLineController : ControllerBase
    {
        private readonly LinkStationAndLineService _service;
        private readonly StationService _stationService;
        private readonly LineService _lineService;


        public LinkStationAndLineController(ILinkStationAndLineRepository linkStationAndLineRepository, IStationRepository stationService, ILineRepository lineService)
        {
            _lineService = new LineService(lineService);
            _service = new LinkStationAndLineService(linkStationAndLineRepository, stationService, lineService);

        }

        /// <summary>
        /// Buscar todos os links
        /// </summary>
       [Authorize(Roles = "administrador,tecnico")]
        [HttpGet("todosLinks")]
        public async Task<ActionResult> BuscarTodos()
        {
            var (result, statusCode) = await _service.GetAllLinkStationAndLineAsync();
            return StatusCode(statusCode, result);
        }

        /// <summary>
        /// Buscar link por ID
        /// </summary>
       [Authorize(Roles = "administrador,tecnico")]
        [HttpGet("{id}")]
        public async Task<ActionResult> BuscarPorId(int id)
        {
            var (result, statusCode) = await _service.GetLinkStationAndLineIdAsync(id);
            return StatusCode(statusCode, result);
        }

        /// <summary>
        /// Buscar links por linha ID
        /// </summary>
       [Authorize(Roles = "administrador,tecnico")]
        [HttpGet("linha-id/{linhaId}")]
        public async Task<ActionResult> LinksPorLinha(int linhaId)
        {
            var (result, statusCode) = await _service.GetLineIdAsync(linhaId);
            return StatusCode(statusCode, result);
        }

        /// <summary>
        /// Buscar estação
        /// </summary>
       [Authorize(Roles = "administrador,tecnico")]
        [HttpGet("estacao-id/{id}")]
        public async Task<ActionResult> LinksPorEstacao(int estacaoId)
        {
            var (result, statusCode) = await _service.GetStationIdAsync(estacaoId);
            return StatusCode(statusCode, result);
        }

        /// <summary>
        /// Criar ou atualizar link
        /// </summary>
        [Authorize(Roles = "administrador,tecnico")]
        [HttpPost("criarLink")]
        public async Task<ActionResult> Incluir([FromBody] LinkStationAndLineModel model)
        {
            var (result, statusCode) = await _service.IncludeAsync(model);
            return StatusCode(statusCode, result);
        }

        /// <summary>
        /// Deletar link
        /// </summary>
       [Authorize(Roles = "administrador,tecnico")]
        [HttpDelete("{id}")]
        public async Task<ActionResult> Deletar(int id)
        {
            var (result, statusCode) = await _service.DeleteAsync(id);
            return StatusCode(statusCode, result);
        }
    }
}
